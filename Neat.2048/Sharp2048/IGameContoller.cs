using Sharp2048;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp2048
{
    public interface IGameContoller
    {
        void Left();
        void Right();
        void Up();
        void Down();
        bool IsFinished();
        int Score { get; }
        void Reset();

        int[,] GetState();

        bool MovedLastTurn();

        int HighestSeenBlock { get; }

        string GetStateString();
    }

    public class Sharp2048GameController : IGameContoller
    {
        private readonly IGameState _gameState;
        private readonly IGameStateHandler _handler;

        private MoveResult _lastMoveResult;
        private int _totalScore = 0;
        private int _highestSeen = 0;

        public Sharp2048GameController(IGameState state, IGameStateHandler handler)
        {
            _gameState = state;
            _handler = handler;
        }

        public void Left()
        {
            _lastMoveResult = _handler.MoveLeft(_gameState);
            _totalScore += _lastMoveResult.Score;
            _highestSeen = Math.Max(_highestSeen, _lastMoveResult.HighestBlock);
            if (_lastMoveResult.StateChange)
            {
                _handler.AddRandomTile(_gameState, _lastMoveResult);
            }
        }

        public void Right()
        {
            _lastMoveResult = _handler.MoveRight(_gameState);
            _totalScore += _lastMoveResult.Score;
            _highestSeen = Math.Max(_highestSeen, _lastMoveResult.HighestBlock);
            if (_lastMoveResult.StateChange)
            {
                _handler.AddRandomTile(_gameState, _lastMoveResult);
            }
        }

        public void Up()
        {
            _lastMoveResult = _handler.MoveUp(_gameState);
            _totalScore += _lastMoveResult.Score;
            _highestSeen = Math.Max(_highestSeen, _lastMoveResult.HighestBlock);
            if (_lastMoveResult.StateChange)
            {
                _handler.AddRandomTile(_gameState, _lastMoveResult);
            }
        }

        public void Down()
        {
            _lastMoveResult = _handler.MoveDown(_gameState);
            _totalScore += _lastMoveResult.Score;
            _highestSeen = Math.Max(_highestSeen, _lastMoveResult.HighestBlock);
            if (_lastMoveResult.StateChange)
            {
                _handler.AddRandomTile(_gameState, _lastMoveResult);
            }
        }

        public bool IsFinished()
        {
            if (_lastMoveResult == null)
            {
                return false;
            }

            return _lastMoveResult.GameOver;
        }

        public int Score
        {
            get { return _totalScore; }
        }


        public void Reset()
        {
            _gameState.Reset();
            _lastMoveResult = null;
            _handler.AddRandomTile(_gameState, null);
            _handler.AddRandomTile(_gameState, null);
            _highestSeen = 0;
            for (int i = 0; i < _gameState.Size; i++)
            {
                var array = _gameState.GetRow(i);
                for (int j = 0; j < _gameState.Size; j++)
                {
                    _highestSeen = Math.Max(_highestSeen, array.GetVal(j));
                }
            }
        }

        public int[,] GetState()
        {
            return _gameState.GetInternalState();
        }


        public bool MovedLastTurn()
        {
            return _lastMoveResult != null && _lastMoveResult.StateChange;
        }


        public int HighestSeenBlock
        {
            get { return _highestSeen; }
        }


        public string GetStateString()
        {
            return _gameState.ToString();
        }
    }
}
