using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharp2048.State;

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
            var runner = new Runner(new Sharp2048GameController(initialState, handler));
            runner.Run();
        }
    }

    public class Runner
    {
        private IGameContoller _controller;
        public Runner(IGameContoller handler)
        {
            _controller = handler;
        }

        public void Run()
        {
            var lastLine = String.Empty;
            do
            {
                System.Console.WriteLine(Convert(_controller.GetState()));

                lastLine = System.Console.ReadLine();
                switch (lastLine.ToLower())
                {
                    case "up":
                        _controller.Up();
                        break;
                    case "down":
                        _controller.Down();
                        break;
                    case "left":
                        _controller.Left();
                        break;
                    case "right":
                        _controller.Right();
                        break;
                    default:
                        System.Console.WriteLine("that isn't a valid direction (up down left right)");
                        break;
                }
            } while (lastLine != "quit");
        }

        public string Convert(int[,] state)
        {
            var builder = new StringBuilder();

            for (int i = 0; i < state.GetLength(0); i++)
            {
                builder.Append("[");
                for (int j = 0; j < state.GetLength(1); j++)
                {
                    builder.Append(state[i,j] + ", ");
                }
                builder.AppendLine("]");
            }
            
            return builder.ToString();
        }
    }
}
