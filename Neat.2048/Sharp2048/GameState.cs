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

        GameArray GetRow(int rowIdx);
        GameArray GetCol(int colIdx);
        void Set(GameArray array, int idx);

        bool IsGameOver();

        int[,] GetInternalState();
    }
    public class GameState : IGameState
    {
        private int[,] _internalArray;
        private int _size;

        public GameState(int size)
        {
            _size = size;
            Reset();
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

        public GameArray GetRow(int rowIdx)
        {
            var result = new GameArray();
            result.Values = new int[_size];
            for (int i = 0; i < _size; i++)
                result.Values[i] = _internalArray[rowIdx, i];
            result.ArrayType = GameArrayType.Row;
            return result;
        }

        public GameArray GetCol(int colIdx)
        {
            var result = new GameArray();
            result.Values = new int[_size];
            for (int i = 0; i < _size; i++)
                result.Values[i] = _internalArray[i, colIdx];
            result.ArrayType = GameArrayType.Column;
            return result;
        }

        public void Set(GameArray array, int idx)
        {
            switch (array.ArrayType)
            {
                case GameArrayType.Column:
                    for (int i = 0; i < _size; i++)
                    {
                        _internalArray[i, idx] = array.Values[i];
                    }
                    break;
                case GameArrayType.Row:
                    for (int i = 0; i < _size; i++)
                    {
                        _internalArray[idx, i] = array.Values[i];
                    }
                    break;
                default:
                    throw new ArgumentException("Unhandled array type" + array.ArrayType, "array");
            }
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
    }

    public class GameArray
    {
        public GameArrayType ArrayType { get; set; }
        public int[] Values { get; set; }
    }

    public enum GameArrayType
    {
        Row,
        Column
    }
}
