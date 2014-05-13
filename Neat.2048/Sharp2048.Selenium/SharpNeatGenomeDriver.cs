using Sharp2048.State;
using SharpNeat.Phenomes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharp2048;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using OpenQA.Selenium.Firefox;

namespace Sharp2048.Selenium
{
    public class SharpNeatGenomeDriver
    {
        private readonly IBlackBox _phenome;
        private string _site;
        private IWebDriver _driver;
        public SharpNeatGenomeDriver(IBlackBox phenome, string site)
        {
            _phenome = phenome;
            _site = site;
            _driver = new FirefoxDriver();
        }

        public void AutoRun()
        {
            _driver.Navigate().GoToUrl(_site);
            var actions = new Actions(_driver);

            var state = GetCurrentState(4);
            _phenome.ResetState();

            while (!IsFinished(state))
            {
                var move = DetermineMove(_phenome, state);
                var element = _driver.FindElement(By.ClassName("grid-container"));
                switch (move)
                {
                    case DriverMove.Left:
                        element.SendKeys(Keys.Left);
                        break;
                    case DriverMove.Right:
                        element.SendKeys(Keys.Right);
                        break;
                    case DriverMove.Up:
                        element.SendKeys(Keys.Up);
                        break;
                    case DriverMove.Down:
                        element.SendKeys(Keys.Down);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                state = GetCurrentState(4);
            }

        }

        public enum DriverMove
        {
            Up,
            Down,
            Left,
            Right
        }

        private bool IsFinished(IGameState state)
        {
            for (int i=0; i<state.Size; i++)
            {
                var row = state.GetRow(i);
                for (int j = 0; j < state.Size; j++)
                {
                    if (row.GetVal(j) == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private DriverMove DetermineMove(IBlackBox box, IGameState state)
        {
            for (int i = 0; i < state.Size; i++)
                for (int j = 0; j < state.Size; j++)
                    box.InputSignalArray[i * state.Size + j] = Math.Log(state.Get(i, j), 2);
            box.ResetState();
            box.Activate();

            var result = DriverMove.Left;
            var crMax = box.OutputSignalArray[0];
            if (box.OutputSignalArray[1] > crMax)
            {
                result = DriverMove.Right;
                crMax = box.OutputSignalArray[1];
            }
            if (box.OutputSignalArray[2] > crMax)
            {
                result = DriverMove.Down;
                crMax = box.OutputSignalArray[2];
            }
            if (box.OutputSignalArray[3] > crMax)
            {
                result = DriverMove.Up;
            }

            return result;
        }

        private IGameState GetCurrentState(int size)
        {
            var gameState = new GameState(size);

            for (int i=1; i<=size; i++)
                for (int j=1; j<=size; j++)
                {
                    try
                    {
                        var val = _driver.FindElement(By.ClassName("tile-container"))
                            .FindElement(By.ClassName(String.Format("tile-position-{0}-{1}", j, i)))
                            .FindElement(By.ClassName("tile-inner"))
                            .Text;
                        gameState.Set(i-1, j-1, int.Parse(val));
                    }
                    catch
                    {
                        // Do Nothing
                    }
                }

            return gameState;
        }
    }
}
