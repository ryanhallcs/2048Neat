using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp2048.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var initialState = new GameState(4);
            var handler = new GameStateHandler();
            handler.AddRandomTile(initialState, null);
            handler.AddRandomTile(initialState, null);
            var runner = new Runner(handler, initialState);
            runner.Run();
        }
    }

    public class Runner
    {
        private IGameState _currentState;
        private IGameStateHandler _handler;
        public Runner(IGameStateHandler handler, IGameState initialState)
        {
            _handler = handler;
            _currentState = initialState;
        }

        public void Run()
        {
            var lastLine = String.Empty;
            do
            {
                System.Console.WriteLine(Convert(_currentState));

                lastLine = System.Console.ReadLine();
                switch (lastLine.ToLower())
                {
                    case "up":
                        _handler.MoveUp(_currentState);
                        break;
                    case "down":
                        _handler.MoveDown(_currentState);
                        break;
                    case "left":
                        _handler.MoveLeft(_currentState);
                        break;
                    case "right":
                        _handler.MoveRight(_currentState);
                        break;
                    default:
                        System.Console.WriteLine("that isn't a valid direction (up down left right)");
                        break;
                }
            } while (lastLine != "quit");
        }

        public string Convert(IGameState state)
        {
            var builder = new StringBuilder();

            for (int i = 0; i < state.Size; i++)
            {
                var row = state.GetRow(i);
                builder.Append("[");
                foreach (var ri in row.Values)
                {
                    builder.Append(ri + ", ");
                }
                builder.AppendLine("]");
            }
            
            return builder.ToString();
        }
    }
}
