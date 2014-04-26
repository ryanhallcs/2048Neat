using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp2048
{
    public interface IGameState
    {
        int Size { get; }
        void Reset();
        void Set(int row, int col, int val);
        int Get(int row, int col);

        IGameArray GetRow(int rowIdx);
        IGameArray GetCol(int colIdx);

        bool IsGameOver();

        int[,] GetInternalState();

        bool[,] GetZeroState();
    }
    public class GameState : IGameState
    {
        private int[,] _internalArray;
        private int _size;

        private IGameArray[] _rows;
        private IGameArray[] _cols;

        public GameState(int size)
        {
            _size = size;
            Reset();
            
            _rows = new IGameArray[size];
            _cols = new IGameArray[size];
            for (int i=0; i<_size; i++)
            {
                _rows[i] = new GameArray(this, i, GameArrayType.Row);
                _cols[i] = new GameArray(this, i, GameArrayType.Column);
            }
        }

        public void Reset()
        {
            _internalArray = new int[_size, _size];
        }

        public void Set(int row, int col, int val)
        {
            _internalArray[row, col] = val;
        }

        public int Get(int row, int col)
        {
            return _internalArray[row, col];
        }

        public int Size
        {
            get { return _size; }
        }

        public IGameArray GetRow(int rowIdx)
        {
            return _rows[rowIdx];
        }

        public IGameArray GetCol(int colIdx)
        {
            return _cols[colIdx];
        }

        public bool IsGameOver()
        {
            foreach (var i in _internalArray)
            {
                if (i == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public int[,] GetInternalState()
        {
            return _internalArray;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            for (int i = 0; i < _size; i++)
                builder.AppendLine(GetRow(i).ToString());
            return builder.ToString();
        }

        public bool[,] GetZeroState()
        {
            throw new NotImplementedException();
        }
    }

    public interface IGameArray
    {
        int Size { get; }
        GameArrayType ArrayType { get; }

        int GetVal(int idx);

        void SetVal(int idx, int val);
    }

    public class GameArray : IGameArray
    {
        private IGameState _stateRef;
        private int _idx;
        private GameArrayType _arrayType;

        public GameArray(IGameState state, int idx, GameArrayType arrayType)
        {
            _stateRef = state;
            _idx = idx;
            _arrayType = arrayType;
        }

        public int Size { get { return _stateRef.Size; } }

        public GameArrayType ArrayType { get { return _arrayType; } }

        public override string ToString()
        {
            return String.Format("[{0}, {1}, {2}, {3}]", GetVal(0), GetVal(1), GetVal(2), GetVal(3));
        }

        public int GetVal(int idx)
        {
            if (ArrayType == GameArrayType.Row)
            {
                return _stateRef.Get(_idx, idx);
            }

            return _stateRef.Get(idx, _idx);
        }

        public void SetVal(int idx, int val)
        {
            if (ArrayType == GameArrayType.Row)
            {
                _stateRef.Set(_idx, idx, val);
            }
            else
            {
                _stateRef.Set(idx, _idx, val);
            }
        }
    }

    public enum GameArrayType
    {
        Row,
        Column
    }
}
