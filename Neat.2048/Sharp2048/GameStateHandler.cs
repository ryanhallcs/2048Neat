using SharpNeat.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sharp2048.State
{
    public interface IGameStateHandler
    {
        MoveResult MoveLeft(IGameState state);
        MoveResult MoveRight(IGameState state);
        MoveResult MoveUp(IGameState state);
        MoveResult MoveDown(IGameState state);
        void ClearGameState(IGameState state);
        void AddRandomTile(IGameState state, MoveResult lastResult);
    }

    public class GameStateHandler : IGameStateHandler
    {
        private readonly FastRandom _rng;

        public GameStateHandler()
        {
            _rng = new FastRandom();
        }
        
        private IGameArray GetRow(IGameState s, int i)
        {
            return s.GetRow(i);
        }
        private IGameArray GetCol(IGameState s, int i)
        {
            return s.GetCol(i);
        }

        private Tuple<int, int> ProcessZeroesRow(int rc, int idx)
        {
            return Tuple.Create(rc, idx);
        }

        private Tuple<int, int> ProcessZeroesCol(int rc, int idx)
        {
            return Tuple.Create(idx, rc);
        }

        public void ClearGameState(IGameState state)
        {
            state.Reset();
        }

        public MoveResult MoveLeft(IGameState state)
        {
            return _move(state, GetRow, ProcessZeroesRow, Direction.Negative, DirectionEnum.Left);
        }

        public MoveResult MoveRight(IGameState state)
        {
            return _move(state, GetRow, ProcessZeroesRow, Direction.Positive, DirectionEnum.Right);
        }

        public MoveResult MoveUp(IGameState state)
        {
            return _move(state, GetCol, ProcessZeroesCol, Direction.Negative, DirectionEnum.Up);
        }

        public MoveResult MoveDown(IGameState state)
        {
            return _move(state, GetCol, ProcessZeroesCol, Direction.Positive, DirectionEnum.Down);
        }

        public void AddRandomTile(IGameState state, MoveResult lastMove)
        {
            List<Tuple<int, int>> possibilities = null;
            if (lastMove == null)
            {
                possibilities = new List<Tuple<int, int>>();
                for (int i = 0; i < state.Size; i++)
                    for (int j = 0; j < state.Size; j++)
                        if (state.Get(i, j) == 0)
                            possibilities.Add(Tuple.Create(i, j));
            }
            else
            {
                possibilities = lastMove.CurrentZeros;
            }

            if (possibilities.Count == 0)
            {
                return;
            }

            int newVal = _rng.NextDouble() < 0.9 ? 2 : 4;
            var newIdx = possibilities[_rng.Next(possibilities.Count)];
            state.Set(newIdx.Item1, newIdx.Item2, newVal);
        }

        private MoveResult _move(IGameState state, Func<IGameState, int, IGameArray> getArray, Func<int, int, Tuple<int,int>> processZeroes, Direction direction, DirectionEnum move)
        {
            var result = new MoveResult { GameOver = true, CurrentZeros = new List<Tuple<int,int>>(), MoveDirection = move };

            bool moved = false;
            for (int i = 0; i < state.Size; i++)
            {
                var gameArray = getArray(state, i);
                var processed = _process(gameArray, direction);
                result.Score += processed.Score;
                result.HighestBlock = Math.Max(processed.HighestBlock, result.HighestBlock);
                result.CurrentZeros.AddRange(processed.CurrentZeros.Select(a => processZeroes(i, a)));
                moved |= processed.Moved || processed.Merged;
            }

            if (moved)
            {
                result.StateChange = true;
            }

            result.GameOver = !result.CurrentZeros.Any();

            return result;
        }

        private ProcessResult _process(IGameArray array, Direction direction)
        {
            var result = new ProcessResult();
            result.CurrentZeros = new List<int>();
            result.Full = true;

            int crnt = 0;
            int delta = 1;
            int max = array.Size;
            int min = -1;
            var merged = new bool[array.Size];
            if (direction == Direction.Positive)
            {
                crnt = array.Size - 1;
                delta = -1;
                max = -1;
                min = array.Size;
            }

            if (array.GetVal(crnt) == 0)
            {
                result.Full = false;
            }

            result.HighestBlock = array.GetVal(crnt);

            for (int i = crnt + delta; i != max; i+=delta )
            {
                if (array.GetVal(i) == 0)
                {
                    result.Full = false;
                    continue;
                }

                result.HighestBlock = Math.Max(result.HighestBlock, array.GetVal(i));

                // Move all the way
                int j = i;
                while (j - delta != min && array.GetVal(j - delta) == 0)
                {
                    j -= delta;
                }
                if (j != i)
                {
                    array.SetVal(j, array.GetVal(i));
                    array.SetVal(i, 0);
                    result.Moved = true;
                    result.Full = false;
                }
                if (j - delta != min && !merged[j - delta] && array.GetVal(j) == array.GetVal(j - delta))
                {
                    array.SetVal(j - delta, array.GetVal(j - delta) * 2);
                    result.Score += array.GetVal(j - delta);
                    array.SetVal(j, 0);
                    merged[j-delta] = true;
                    result.Merged = true;
                    result.Full = false;
                }
            }

            for (int i = 0; i < array.Size; i++)
            {
                if (array.GetVal(i) == 0)
                {
                    result.CurrentZeros.Add(i);
                }
            }

            return result;
        }

        public enum Direction
        {
            Positive,
            Negative
        }

        public class ProcessResult
        {
            public bool Merged { get; set; }
            public bool Moved { get; set; }

            public int Score { get; set; }
            public bool Full { get; set; }

            public int HighestBlock { get; set; }

            public List<int> CurrentZeros { get; set; }
        }
    }

    public class MoveResult
    {
        public int Score { get; set; }

        public DirectionEnum MoveDirection { get; set; }

        public int HighestBlock { get; set; }

        public bool GameOver { get; set; }

        public bool StateChange { get; set; }

        public List<Tuple<int, int>> CurrentZeros { get; set; }
    }
}
