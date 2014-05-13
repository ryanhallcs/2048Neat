using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharp2048.Neat.Service.Models;
using Sharp2048.State;
using SharpNeat.Genomes.Neat;
using SharpNeat.Phenomes;
using DirectionEnum = Sharp2048.State.DirectionEnum;

namespace Sharp2048.Neat.Service
{
    public interface IGenome2048Ai
    {
        DirectionEnum GetNextMove(IBlackBox phenome, IGameContoller controller);
    }

    public class BoardEvaluatorMinimax2048Ai : IGenome2048Ai
    {
        private int _maxDepth;

        public BoardEvaluatorMinimax2048Ai(int maxDepth)
        {
            _maxDepth = maxDepth;
        }

        public DirectionEnum GetNextMove(IBlackBox phenome, IGameContoller controller)
        {
            return _bestDepth(phenome, controller, _maxDepth).Item1;
        }

        private static List<DirectionEnum> _allDirs = Enum.GetValues(typeof(DirectionEnum)).Cast<DirectionEnum>().ToList();

        private Tuple<DirectionEnum, double> _bestDepth(IBlackBox phenome, IGameContoller currentController, int depth)
        {
            if (depth == 0 || currentController.IsFinished())
            {
                return Tuple.Create(currentController.LastMove.MoveDirection, _evaluateState(phenome, currentController));
            }
            if (currentController.LastMove != null && !currentController.MovedLastTurn())
            {
                return Tuple.Create(currentController.LastMove.MoveDirection, -97654321.0);
            }
            Tuple<DirectionEnum, double> result = null;

            foreach (var moveEnum in _allDirs)
            {
                var cloned = currentController.Clone();
                switch (moveEnum)
                {
                    case DirectionEnum.Up:
                        cloned.Up();
                        break;
                    case DirectionEnum.Right:
                        cloned.Right();
                        break;
                    case DirectionEnum.Down:
                        cloned.Down();
                        break;
                    case DirectionEnum.Left:
                        cloned.Left();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var thisRes = _bestDepth(phenome, cloned, depth - 1);
                if (result == null || thisRes.Item2 > result.Item2)
                {
                    result = thisRes;
                }
            }

            return result;
        }

        public double _evaluateState(IBlackBox phenome, IGameContoller gameController)
        {
            var state = gameController.GetState();
            var n = state.GetLength(0);
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    phenome.InputSignalArray[i * n + j] = state[i, j];// Math.Log(state[i, j], 2);

            phenome.ResetState();
            phenome.Activate();

            return phenome.OutputSignalArray[0];
        }
    }

    public class DirectMover2048Ai : IGenome2048Ai
    {
        public DirectionEnum GetNextMove(IBlackBox phenome, IGameContoller controller)
        {
            var state = controller.GetState();
            var n = state.GetLength(0);
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    phenome.InputSignalArray[i * n + j] = state[i, j];

            phenome.ResetState();
            phenome.Activate();

            var crMax = phenome.OutputSignalArray[0];
            var result = DirectionEnum.Up;
            if (phenome.OutputSignalArray[1] > crMax)
            {
                result = DirectionEnum.Right;
                crMax = phenome.OutputSignalArray[1];
            }
            if (phenome.OutputSignalArray[2] > crMax)
            {
                result = DirectionEnum.Down;
                crMax = phenome.OutputSignalArray[2];
            }
            if (phenome.OutputSignalArray[3] > crMax)
            {
                result = DirectionEnum.Left;
            }

            return result;
        }
    }
}
