using Sharp2048.Neat.Service;
using Sharp2048.State;
using log4net;
using Sharp2048;
using SharpNeat.Core;
using SharpNeat.Phenomes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpNeat.Domains
{
    public class Sharp2048Evaluator : IPhenomeEvaluator<IBlackBox>
    {
        private static readonly ILog __log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Func<IGameContoller> _gameController;
        private long _evalCount;
        private const int _maxNoChange = 3;
        private const int _runsPerEval = 3;
        private readonly IGenome2048Ai _evaluator;

        public Sharp2048Evaluator(Func<IGameContoller> controllerFactory, IGenome2048Ai evaluator)
        {
            _gameController = controllerFactory;
            _evaluator = evaluator;
        }
        public ulong EvaluationCount
        {
            get { return (ulong)_evalCount; }
        }

        public bool StopConditionSatisfied
        {
            get { return false; }
        }

        public FitnessInfo Evaluate(IBlackBox phenome)
        {
            Interlocked.Increment(ref _evalCount);
            //var crntEval = Interlocked.Read(ref _evalCount);

            var fitness = 0d;
            var altFitness = 0d;
            for (int i = 0; i<_runsPerEval; i++)
            {
                var res = _evaluateOne(phenome);
                fitness += res.Item1;
                altFitness += res.Item2;

            };

            fitness /= _runsPerEval;
            altFitness /= _runsPerEval;

            //__log.InfoFormat("Evaluating #{0}: [{1} :: {2}]", crntEval, fitness + altFitness, altFitness);
            return new FitnessInfo(fitness + altFitness, altFitness);
        }

        private Tuple<int, double> _evaluateOne(IBlackBox phenome)
        {
            var gameController = _gameController();
            gameController.Reset();
            var noChange = 0;
            while (!gameController.IsFinished() && noChange < _maxNoChange)
            {
                var result = _evaluator.GetNextMove(phenome, gameController);

                switch (result)
                {
                    case DirectionEnum.Up:
                        gameController.Up();
                        break;
                    case DirectionEnum.Right:
                        gameController.Right();
                        break;
                    case DirectionEnum.Down:
                        gameController.Down();
                        break;
                    case DirectionEnum.Left:
                        gameController.Left();
                        break;
                    default:
                        throw new Exception("Incorrect output");
                }

                if (!gameController.MovedLastTurn())
                {
                    noChange++;
                }
            }

            var fitness = gameController.Score;
            double altFitness = gameController.Score + gameController.HighestSeenBlock;
            if (noChange < _maxNoChange)
            {
                altFitness *= 1.1d;
            }

            return Tuple.Create(fitness, altFitness); // alt fitness takes highest block into account and gives a small boost for getting into a final state
        }

        public void Reset()
        {
            _evalCount = 0;
        }
    }
}
