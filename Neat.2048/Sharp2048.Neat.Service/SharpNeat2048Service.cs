using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Sharp2048.Data;
using Sharp2048.Neat.Service.Models;
using Sharp2048.State;
using SharpNeat.Core;
using SharpNeat.Genomes.Neat;
using SharpNeat.Network;
using SharpNeat.Phenomes;
using SharpNeat.Utility;
using DirectionEnum = Sharp2048.State.DirectionEnum;

namespace Sharp2048.Neat.Service
{
    public interface ISharpNeat2048Service
    {
        Genome GetGenome(Guid genomeId);
        Genome SaveNewGenome(string description, string loadedBy, string rawGenome, string evaluatorType);
        IBlackBox GetDecodedGenome(Guid genomeId);
        DirectionEnum ProcessMove(IGameContoller state, Guid genomeId);
    }

    public class SharpNeat2048Service : ISharpNeat2048Service
    {
        private readonly Sharp2048DataModelContainer _neatDb;
        private readonly IGenomeDecoder<NeatGenome, IBlackBox> _decoder;
        private readonly IGenomeGenerator _generator;
        private readonly IEvaluatorFactory _evalFactory;
        public SharpNeat2048Service(Sharp2048DataModelContainer neatDb, IGenomeDecoder<NeatGenome, IBlackBox> decoder, IGenomeGenerator generator, IEvaluatorFactory evalFactory)
        {
            _neatDb = neatDb;
            _decoder = decoder;
            _generator = generator;
            _evalFactory = evalFactory;
        }

        public Genome GetGenome(Guid genomeId)
        {
            return _neatDb.Genomes.SingleOrDefault(a => a.GenomeIdentifier == genomeId);
        }

        public Genome SaveNewGenome(string description, string loadedBy, string rawGenome, string evaluatorType)
        {
            var genome = _trySaveSingle(rawGenome) 
                ?? _trySaveSingleList(rawGenome)
                ?? _trySaveSingleFullList(rawGenome);

            if (genome == null)
            {
                return null;
            }
            Type evalType;
            try
            {
                evalType = Type.GetType(evaluatorType);
            }
            catch (Exception)
            {
                return null;
            }

            if (evalType == null || !typeof(IGenome2048Ai).IsAssignableFrom(typeof(IGenome2048Ai)))
            {
                return null;
            }

            var stringBuilder = new StringBuilder();
            using (var writer = XmlWriter.Create(stringBuilder))
            {
                NeatGenomeXmlIO.Write(writer, genome, false);
            }

            var newGenome = new Genome
            {
                GenomeIdentifier = Guid.NewGuid(),
                Description = description,
                GenomeXml = stringBuilder.ToString(),
                EvaluatorType = evalType.ToString()
            };
            _neatDb.Genomes.Add(newGenome);
            _neatDb.SaveChanges();
            return GetGenome(newGenome.GenomeIdentifier);
        }

        private NeatGenome _trySaveSingle(string rawGenome)
        {
            try
            {
                return _generator.GenerateFromXmlString(rawGenome);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private NeatGenome _trySaveSingleList(string rawGenome)
        {
            try
            {
                return _generator.GenerateSimpleListFromXmlString(rawGenome).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        private NeatGenome _trySaveSingleFullList(string rawGenome)
        {
            try
            {
                return _generator.GenerateCompleteListFromXmlString(rawGenome).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IBlackBox GetDecodedGenome(Guid genomeId)
        {
            var genome = GetGenome(genomeId);

            var neatGenome = _generator.GenerateFromXmlString(genome.GenomeXml);

            return _decoder.Decode(neatGenome);
        }

        public DirectionEnum ProcessMove(IGameContoller state, Guid genomeId)
        {
            IGenome2048Ai processor = null;
            var evalType = GetGenome(genomeId).EvaluatorType;
            var phenome = GetDecodedGenome(genomeId);
            try
            {
                processor = _evalFactory.Create(evalType);
                return processor.GetNextMove(phenome, state);
            }
            finally
            {
                _evalFactory.Release(processor);
            }
        }
    }
}
