using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp2048.Tests
{
    [TestFixture]
    public class GameControllerTests
    {
        private class Context
        {
            public IGameState GameState { get; set; }

            public IGameStateHandler Handler { get; set; }

            public Sharp2048GameController Sut { get; set; }

            public Context()
            {
                GameState = Substitute.For<IGameState>();
                Handler = Substitute.For<IGameStateHandler>();
                Sut = new Sharp2048GameController(GameState, Handler);
            }
        }

        [Test]
        public void TestInitialState()
        {
            var context = new Context();

            Assert.That(context.Sut.HighestSeenBlock, Is.EqualTo(0));
            Assert.That(context.Sut.IsFinished(), Is.False);
            Assert.That(context.Sut.MovedLastTurn(), Is.False);
            Assert.That(context.Sut.Score, Is.EqualTo(0));
        }

        [Test]
        public void TestScoreUpdatedOnChange()
        {
            var context = new Context();

            var result = new MoveResult();
            result.Score = 2;

            context.Handler.MoveLeft(null).ReturnsForAnyArgs(result);
            context.Sut.Left();
            Assert.That(context.Sut.Score, Is.EqualTo(2));
        }

        [Test]
        public void TestScoreAggregateOnChange()
        {
            var context = new Context();

            var result = new MoveResult();
            result.Score = 2;

            context.Handler.MoveLeft(null).ReturnsForAnyArgs(result);
            context.Sut.Left();
            Assert.That(context.Sut.Score, Is.EqualTo(2));
            result.Score = 4;
            context.Sut.Left();
            Assert.That(context.Sut.Score, Is.EqualTo(6));
            result.Score = 2;
            context.Sut.Left();
            Assert.That(context.Sut.Score, Is.EqualTo(8));
        }

        [Test]
        public void TestResetState()
        {
            var context = new Context();
            var resultArray = CreateArray(new[] { 0, 0, 0, 0 });
            context.GameState.GetRow(1).ReturnsForAnyArgs(resultArray);
            context.Sut.Reset();
            Assert.That(context.Sut.HighestSeenBlock, Is.EqualTo(0));
            Assert.That(context.Sut.IsFinished(), Is.False);
            Assert.That(context.Sut.MovedLastTurn(), Is.False);
            Assert.That(context.Sut.Score, Is.EqualTo(0));
        }

        [Test]
        public void TestResetStateWithHighVal()
        {
            var context = new Context();
            var resultArray = CreateArray(new[] { 2, 4, 5, 6 });
            context.GameState.GetRow(1).ReturnsForAnyArgs(resultArray);
            context.GameState.Size.ReturnsForAnyArgs(4);
            context.Sut.Reset();
            Assert.That(context.Sut.HighestSeenBlock, Is.EqualTo(6));
            Assert.That(context.Sut.IsFinished(), Is.False);
            Assert.That(context.Sut.MovedLastTurn(), Is.False);
            Assert.That(context.Sut.Score, Is.EqualTo(0));
        }

        IGameArray CreateArray(int[] vals)
        {
            var result = Substitute.For<IGameArray>();

            for (int i=0; i < vals.Length; i++)
            {
                result.GetVal(i).Returns(vals[i]);
            }
            
            return result;
        }

        [Test]
        public void TestIsFinishedOnFinishedState()
        {
            var context = new Context();

            var result = new MoveResult();
            result.GameOver = true;

            context.Handler.MoveRight(null).ReturnsForAnyArgs(result);
            context.Sut.Right();
            Assert.That(context.Sut.IsFinished(), Is.True);
        }
    }
}
