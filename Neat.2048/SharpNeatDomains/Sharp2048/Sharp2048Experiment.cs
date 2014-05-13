using Sharp2048.Neat.Service;
using Sharp2048.State;
using SharpNeat.Decoders.HyperNeat;
using SharpNeat.Genomes.HyperNeat;
using SharpNeat.Network;
using log4net;
using Sharp2048;
using SharpNeat.Core;
using SharpNeat.Decoders;
using SharpNeat.Decoders.Neat;
using SharpNeat.DistanceMetrics;
using SharpNeat.Domains;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.Genomes.Neat;
using SharpNeat.Phenomes;
using SharpNeat.SpeciationStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SharpNeat.Domains
{
    public class Sharp2048Experiment : IGuiNeatExperiment
    {
        private static readonly ILog __log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        NeatEvolutionAlgorithmParameters _eaParams;
        NeatGenomeParameters _neatGenomeParams; 
        int _populationSize;
        int _specieCount;
        NetworkActivationScheme _activationSchemeCppn;
        NetworkActivationScheme _activationScheme;
        string _complexityRegulationStr;
        int? _complexityThreshold;
        string _variantStr;
        string _description;
        ParallelOptions _parallelOptions;
        private bool _lengthCppnInput;
        private bool _useHyperNeat;
        private bool _isBoardEvaluator;

        private int _maxDepth = 4;
        private int _boardLength;

        public AbstractGenomeView CreateGenomeView()
        {
            return _useHyperNeat
            ? (AbstractGenomeView) new CppnGenomeView(GetCppnActivationFunctionLibrary()) 
            : new NeatGenomeView();
        }

        public AbstractDomainView CreateDomainView()
        {
            return null;
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public int InputCount { get { return _useHyperNeat ? (_lengthCppnInput ? 7 : 6) : 16; } }
        public int OutputCount { get { return _useHyperNeat ? 2 : 1; } }

        public int DefaultPopulationSize { get; set; }

        public NeatEvolutionAlgorithmParameters NeatEvolutionAlgorithmParameters
        {
            get { return _eaParams; }
        }

        /// <summary>
        /// Gets the NeatGenomeParameters to be used for the experiment. Parameters on this object can be modified. Calls
        /// to CreateEvolutionAlgorithm() make a copy of and use this object in whatever state it is in at the time of the call.
        /// </summary>
        public NeatGenomeParameters NeatGenomeParameters
        {
            get { return _neatGenomeParams; }
        }

        public void Initialize(string name, XmlElement xmlConfig)
        {
            Name = name;

            _populationSize = XmlUtils.GetValueAsInt(xmlConfig, "PopulationSize");
            _specieCount = XmlUtils.GetValueAsInt(xmlConfig, "SpecieCount");
            _activationScheme = ExperimentUtils.CreateActivationScheme(xmlConfig, "Activation");
            _activationSchemeCppn = ExperimentUtils.CreateActivationScheme(xmlConfig, "ActivationCppn");
            _complexityRegulationStr = XmlUtils.TryGetValueAsString(xmlConfig, "ComplexityRegulationStrategy");
            _complexityThreshold = XmlUtils.TryGetValueAsInt(xmlConfig, "ComplexityThreshold");
            _variantStr = XmlUtils.TryGetValueAsString(xmlConfig, "Variant");
            _description = XmlUtils.TryGetValueAsString(xmlConfig, "Description");
            _parallelOptions = ExperimentUtils.ReadParallelOptions(xmlConfig);
            _lengthCppnInput = XmlUtils.GetValueAsBool(xmlConfig, "LengthCppnInput");
            _useHyperNeat = XmlUtils.GetValueAsBool(xmlConfig, "UseHyperNeat");
            _boardLength = XmlUtils.GetValueAsInt(xmlConfig, "BoardLength");
            _maxDepth = XmlUtils.TryGetValueAsInt(xmlConfig, "MaxDepth") ?? _maxDepth;
            _isBoardEvaluator = XmlUtils.GetValueAsBool(xmlConfig, "BoardEvaluator");

            _eaParams = new NeatEvolutionAlgorithmParameters();
            _eaParams.SpecieCount = _specieCount;
            _neatGenomeParams = new NeatGenomeParameters();

            _neatGenomeParams.AddConnectionMutationProbability = 0.1;
            _neatGenomeParams.AddNodeMutationProbability = 0.01;
            _neatGenomeParams.ConnectionWeightMutationProbability = 0.9865;
            _neatGenomeParams.DeleteConnectionMutationProbability = 0.01;
            _neatGenomeParams.DeleteSimpleNeuronMutationProbability = 0.001;

            DefaultPopulationSize = _populationSize;
            Description = _description;
        }

        /// <summary>
        /// Load a population of genomes from an XmlReader and returns the genomes in a new list.
        /// The genome factory for the genomes can be obtained from any one of the genomes.
        /// </summary>
        public List<NeatGenome> LoadPopulation(XmlReader xr)
        {
            return NeatGenomeUtils.LoadPopulation(xr, false, this.InputCount, this.OutputCount, CreateGenomeFactory());
        }

        /// <summary>
        /// Save a population of genomes to an XmlWriter.
        /// </summary>
        public void SavePopulation(XmlWriter xw, IList<NeatGenome> genomeList)
        {
            // Writing node IDs is not necessary for NEAT.
            NeatGenomeXmlIO.WriteComplete(xw, genomeList, false);
        }

        public IGenomeDecoder<NeatGenome, IBlackBox> CreateGenomeDecoder()
        {
            return _useHyperNeat 
                ? CreateHyperNeatGenomeDecoder() 
                : new NeatGenomeDecoder(_activationScheme);
        }

        public IGenomeDecoder<NeatGenome, IBlackBox> CreateHyperNeatGenomeDecoder()
        {
            var boardSize = _boardLength * _boardLength;
            // Create 3 layer 'sandwich' substrate.
            var inputLayer = new SubstrateNodeSet();
            var hiddenLayer = new SubstrateNodeSet();
            var outputLayer = new SubstrateNodeSet(); 

            // Node IDs start at 1. (bias node is always zero).
            uint inputId = 1;
            uint outputId = (uint)(boardSize + inputId);
            uint hiddenId = (uint)(outputId + 1);

            for (int y = 0; y < _boardLength; y++)
            {
                for (int x = 0; x < _boardLength; x++, inputId++, hiddenId++)
                {
                    inputLayer.NodeList.Add(new SubstrateNode(inputId, new double[] { x, y, -1.0 }));
                    hiddenLayer.NodeList.Add(new SubstrateNode(hiddenId, new double[] { x, y, 0.0 }));
                }
            }

            // Just 1 output - the board evaluation result
            outputLayer.NodeList.Add(new SubstrateNode(outputId, new double[] { 0, 0, 1.0 }));

            var nodeSetList = new List<SubstrateNodeSet>();
            nodeSetList.Add(inputLayer);
            nodeSetList.Add(outputLayer);
            nodeSetList.Add(hiddenLayer);

            // Define connection mappings between layers/sets.
            var nodeSetMappingList = new List<NodeSetMapping>();
            nodeSetMappingList.Add(NodeSetMapping.Create(0, 2, (double?)null));
            nodeSetMappingList.Add(NodeSetMapping.Create(2, 1, (double?)null));


            // Construct substrate.
            var substrate = new Substrate(nodeSetList, GetCppnActivationFunctionLibrary(), 0, 0.2, 5, nodeSetMappingList);

            // Create genome decoder. Decodes to a neural network packaged with an activation scheme that defines a fixed number of activations per evaluation.
            var genomeDecoder = new HyperNeatDecoder(substrate, _activationSchemeCppn, _activationScheme, _lengthCppnInput);
            return genomeDecoder;
        }

        /// <summary>
        /// Create a genome factory for the experiment.
        /// Create a genome factory with our neat genome parameters object and the appropriate number of input and output neuron genes.
        /// </summary>
        public IGenomeFactory<NeatGenome> CreateGenomeFactory()
        {
            return _useHyperNeat
            ? new CppnGenomeFactory(InputCount, OutputCount, GetCppnActivationFunctionLibrary(), _neatGenomeParams)
            : new NeatGenomeFactory(InputCount, OutputCount, _neatGenomeParams);
        }

        /// <summary>
        /// Create and return a NeatEvolutionAlgorithm object ready for running the NEAT algorithm/search. Various sub-parts
        /// of the algorithm are also constructed and connected up.
        /// Uses the experiments default population size defined in the experiment's config XML.
        /// </summary>
        public NeatEvolutionAlgorithm<NeatGenome> CreateEvolutionAlgorithm()
        {
            return CreateEvolutionAlgorithm(_populationSize);
        }

        public NeatEvolutionAlgorithm<NeatGenome> CreateEvolutionAlgorithm(int populationSize)
        {
            // Create a genome factory with our neat genome parameters object and the appropriate number of input and output neuron genes.
            IGenomeFactory<NeatGenome> genomeFactory = CreateGenomeFactory();

            // Create an initial population of randomly generated genomes.
            List<NeatGenome> genomeList = genomeFactory.CreateGenomeList(populationSize, 0);

            // Create evolution algorithm.
            return CreateEvolutionAlgorithm(genomeFactory, genomeList);
        }

        public NeatEvolutionAlgorithm<NeatGenome> CreateEvolutionAlgorithm(IGenomeFactory<NeatGenome> genomeFactory, List<NeatGenome> genomeList)
        {
            // Create distance metric. Mismatched genes have a fixed distance of 10; for matched genes the distance is their weigth difference.
            var distanceMetric = new ManhattanDistanceMetric(1.0, 0.0, 10.0);
            var speciationStrategy = new ParallelKMeansClusteringStrategy<NeatGenome>(distanceMetric, _parallelOptions);

            // Create complexity regulation strategy.
            var complexityRegulationStrategy = ExperimentUtils.CreateComplexityRegulationStrategy(_complexityRegulationStr, _complexityThreshold);

            // Create the evolution algorithm.
            var ea = new NeatEvolutionAlgorithm<NeatGenome>(_eaParams, speciationStrategy, complexityRegulationStrategy);
            
            // Create IBlackBox evaluator.
            IGenome2048Ai boardEvaluator = null;
            if (_isBoardEvaluator)
            {
                boardEvaluator = new BoardEvaluatorMinimax2048Ai(_maxDepth);
            }
            else
            {
                boardEvaluator = new DirectMover2048Ai();
            }
            var evaluator = new Sharp2048Evaluator(() => new Sharp2048GameController(new GameState(_boardLength), new GameStateHandler()), boardEvaluator);

            // Create genome decoder.
            var genomeDecoder = CreateGenomeDecoder();

            // Create a genome list evaluator. This packages up the genome decoder with the genome evaluator.
            var innerEvaluator = new SerialGenomeListEvaluator<NeatGenome, IBlackBox>(genomeDecoder, evaluator);
            //var innerEvaluator = new ParallelGenomeListEvaluator<NeatGenome, IBlackBox>(genomeDecoder, evaluator, _parallelOptions);

            // Wrap the list evaluator in a 'selective' evaulator that will only evaluate new genomes. That is, we skip re-evaluating any genomes
            // that were in the population in previous generations (elite genomes). This is determiend by examining each genome's evaluation info object.
            var selectiveEvaluator = new SelectiveGenomeListEvaluator<NeatGenome>(
                                                                                    innerEvaluator,
                                                                                    SelectiveGenomeListEvaluator<NeatGenome>.CreatePredicate_OnceOnly());
            // Initialize the evolution algorithm.
            ea.Initialize(selectiveEvaluator, genomeFactory, genomeList);

            // Finished. Return the evolution algorithm
            return ea;
        }
            
        private IActivationFunctionLibrary GetCppnActivationFunctionLibrary()
        {
            return DefaultActivationFunctionLibrary.CreateLibraryCppn();
        }
    }
}
