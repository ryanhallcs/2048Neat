using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp2048
{
    public interface IGameStateHandler
    {
        void MoveLeft(IGameState state);
        void MoveRight(IGameState state);
        void MoveUp(IGameState state);
        void MoveDown(IGameState state);
        void ClearGameState(IGameState state);
        void AddRandomTile(IGameState state);
    }

    public class GameStateHandler:IGameStateHandler
    {
        private readonly Random _rng;

        public GameStateHandler()
        {
            _rng = new Random();
        }

        public void ClearGameState(IGameState state)
        {
            state.Reset();
        }

        public void MoveLeft(IGameState state)
        {
            bool moved = false;
            for (int i=0; i<state.Size; i++)
            {
                var gameArray = state.GetRow(i);
                var result = _process(gameArray, Direction.Negative);
                if (result.Moved || result.Merged)
                {
                    state.Set(gameArray, i);
                    moved = true;
                }
            }

            if (moved)
            {
                AddRandomTile(state);
            }
        }

        public void MoveRight(IGameState state)
        {
            bool moved = false;
            for (int i = 0; i < state.Size; i++)
            {
                var gameArray = state.GetRow(i);
                var result = _process(gameArray, Direction.Positive);
                if (result.Moved || result.Merged)
                {
                    state.Set(gameArray, i);
                    moved = true;
                }
            }

            if (moved)
            {
                AddRandomTile(state);
            }
        }

        public void MoveUp(IGameState state)
        {
            bool moved = false;
            for (int i = 0; i < state.Size; i++)
            {
                var gameArray = state.GetCol(i);
                var result = _process(gameArray, Direction.Negative);
                if (result.Moved || result.Merged)
                {
                    state.Set(gameArray, i);
                    moved = true;
                }
            }

            if (moved)
            {
                AddRandomTile(state);
            }
        }

        public void MoveDown(IGameState state)
        {
            bool moved = false;
            for (int i = 0; i < state.Size; i++)
            {
                var gameArray = state.GetCol(i);
                var result = _process(gameArray, Direction.Positive);
                if (result.Moved || result.Merged)
                {
                    state.Set(gameArray, i);
                    moved = true;
                }
            }

            if (moved)
            {
                AddRandomTile(state);
            }
        }

        public void AddRandomTile(IGameState state)
        {
            var possibilities = new List<Tuple<int, int>>();
            for (int i=0; i<state.Size; i++)
            {
                for (int j=0; j<state.Size; j++)
                {
                    if (state.Get(i, j) == 0)
                    {
                        possibilities.Add(new Tuple<int, int>(i, j));
                    }
                }
            }

            if (possibilities.Count == 0)
            {
                return;
            }

            int newVal = _rng.NextDouble() < 0.9 ? 2 : 4;
            var newIdx = possibilities[_rng.Next(possibilities.Count)];
            state.Set(newIdx.Item1, newIdx.Item2, newVal);
        }

        private ProcessResult _process(GameArray array, Direction direction)
        {
            var result = new ProcessResult();

            var merged = new bool[array.Values.Length];
            int crnt = 0;
            int delta = 1;
            int max = array.Values.Length;
            int min = -1;
            if (direction == Direction.Positive)
            {
                crnt = array.Values.Length - 1;
                delta = -1;
                max = -1;
                min = array.Values.Length;
            }

            for (int i = crnt + delta; i != max; i+=delta )
            {
                if (array.Values[i] == 0)
                {
                    continue;
                } 
                
                // Move all the way
                int j = i;
                while (j - delta != min && array.Values[j - delta] == 0)
                {
                    j -= delta;
                }
                if (j != i)
                {
                    array.Values[j] = array.Values[i];
                    array.Values[i] = 0;
                    result.Moved = true;
                }
                if (j-delta != min && !merged[j-delta] && array.Values[j] == array.Values[j-delta])
                {
                    array.Values[j - delta] *= 2;
                    array.Values[j] = 0;
                    merged[j-delta] = true;
                    result.Merged = true;
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
        }
    }
}
