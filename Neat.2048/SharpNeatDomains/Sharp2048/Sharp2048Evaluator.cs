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
        private readonly Func<IGameContoller> _gameController;
        private long _evalCount;
        private const int _maxIter = 2000;
        private const int _maxNoChange = 5;
        private const int _runsPerEval = 4;

        public Sharp2048Evaluator(Func<IGameContoller> controllerFactory)
        {
            _gameController = controllerFactory;
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
            var fitness = 0d;
            for (int i = 0; i<_runsPerEval; i++)
            {
                fitness += _evaluateOne(phenome);
            };

            fitness /= _runsPerEval;
            return new FitnessInfo(fitness, fitness);
        }

        public double _evaluateOne(IBlackBox phenome)
        {
            phenome.ResetState();
            var gameController = _gameController();
            gameController.Reset();
            var fitness = 0;
            var noChange = 0;
            while (!gameController.IsFinished() && noChange < _maxNoChange)
            {
                var state = gameController.GetState();
                var n = state.GetLength(0);
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        phenome.InputSignalArray[i * n + j] = state[i, j];

                phenome.Activate();

                var crMax = phenome.OutputSignalArray[0];
                int idx = 0;
                if (phenome.OutputSignalArray[1] > crMax)
                {
                    idx = 1;
                    crMax = phenome.OutputSignalArray[1];
                }
                if (phenome.OutputSignalArray[2] > crMax)
                {
                    idx = 2;
                    crMax = phenome.OutputSignalArray[2];
                }
                if (phenome.OutputSignalArray[3] > crMax)
                {
                    idx = 3;
                }

                switch (idx)
                {
                    case 0:
                        gameController.Left();
                        break;
                    case 1:
                        gameController.Right();
                        break;
                    case 2:
                        gameController.Down();
                        break;
                    case 3:
                        gameController.Up();
                        break;
                    default:
                        throw new Exception("Incorrect output");
                }

                if (gameController.MovedLastTurn())
                {
                    fitness++;
                }
                else
                {
                    noChange++;
                }
            }
            return gameController.Score;
        }

        public void Reset()
        {
            _evalCount = 0;
        }
    }
}
