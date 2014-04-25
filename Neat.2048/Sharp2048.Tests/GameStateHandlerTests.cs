using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharp2048;

namespace Sharp2048.Tests
{
    [TestFixture]
    public class GameStateHandlerTests
    {
        [Test]
        public void MoveLeftSuccessNoMerge()
        {
            int size = 4;

            var sut = new GameStateHandler();
            var gameState = new GameState(size);
            gameState.Set(0, 1, 1);
            sut.MoveLeft(gameState);

            Assert.That(gameState.Get(0, 0), Is.EqualTo(1));

            gameState = new GameState(size);
            gameState.Set(0, 2, 1);
            sut.MoveLeft(gameState);

            Assert.That(gameState.Get(0, 0), Is.EqualTo(1));

            gameState = new GameState(size);
            gameState.Set(0, 3, 1);
            sut.MoveLeft(gameState);

            Assert.That(gameState.Get(0, 0), Is.EqualTo(1));
        }

        [Test]
        public void MoveRightSuccessNoMerge()
        {
            int size = 4;

            var sut = new GameStateHandler();
            var gameState = new GameState(size);
            gameState.Set(0, 1, 1);
            sut.MoveRight(gameState);

            Assert.That(gameState.Get(0, size - 1), Is.EqualTo(1));

            gameState = new GameState(size);
            gameState.Set(0, 2, 1);
            sut.MoveRight(gameState);

            Assert.That(gameState.Get(0, size - 1), Is.EqualTo(1));

            gameState = new GameState(size);
            gameState.Set(0, 0, 1);
            sut.MoveRight(gameState);

            Assert.That(gameState.Get(0, size - 1), Is.EqualTo(1));
        }

        [Test]
        public void MoveDownSuccessNoMerge()
        {
            int size = 4;

            var sut = new GameStateHandler();
            var gameState = new GameState(size);
            gameState.Set(0, 0, 1);
            sut.MoveDown(gameState);

            Assert.That(gameState.Get(size - 1, 0), Is.EqualTo(1));

            gameState = new GameState(size);
            gameState.Set(1, 0, 1);
            sut.MoveDown(gameState);

            Assert.That(gameState.Get(size - 1, 0), Is.EqualTo(1));

            gameState = new GameState(size);
            gameState.Set(2, 0, 1);
            sut.MoveDown(gameState);

            Assert.That(gameState.Get(size - 1, 0), Is.EqualTo(1));
        }

        [Test]
        public void MoveLeftSuccessMerge()
        {
            int size = 4;

            var sut = new GameStateHandler();
            var gameState = new GameState(size);
            gameState.Set(0, 1, 2);
            gameState.Set(0, 0, 2);
            sut.MoveLeft(gameState);

            Assert.That(gameState.Get(0, 0), Is.EqualTo(4));
            int cnt = 0;
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (gameState.Get(i, j) != 0)
                        cnt++;
            Assert.That(cnt, Is.EqualTo(2));

            gameState = new GameState(size);
            gameState.Set(0, 1, 2);
            gameState.Set(0, 2, 2);
            sut.MoveLeft(gameState);

            Assert.That(gameState.Get(0, 0), Is.EqualTo(4));
            cnt = 0;
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (gameState.Get(i, j) != 0)
                        cnt++;
            Assert.That(cnt, Is.EqualTo(2));

            gameState = new GameState(size);
            gameState.Set(0, 1, 2);
            gameState.Set(0, 3, 2);
            sut.MoveLeft(gameState);

            Assert.That(gameState.Get(0, 0), Is.EqualTo(4));
            cnt = 0;
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (gameState.Get(i, j) != 0)
                        cnt++;
            Assert.That(cnt, Is.EqualTo(2));
        }

        [Test]
        public void MoveRightSuccessMerge()
        {
            int size = 4;

            var sut = new GameStateHandler();
            var gameState = new GameState(size);
            gameState.Set(0, 1, 2);
            gameState.Set(0, 0, 2);
            sut.MoveRight(gameState);

            Assert.That(gameState.Get(0, size - 1), Is.EqualTo(4));
            int cnt = 0;
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (gameState.Get(i, j) != 0)
                        cnt++;
            Assert.That(cnt, Is.EqualTo(2));

            gameState = new GameState(size);
            gameState.Set(0, 1, 2);
            gameState.Set(0, 2, 2);
            sut.MoveRight(gameState);

            Assert.That(gameState.Get(0, size - 1), Is.EqualTo(4));
            cnt = 0;
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (gameState.Get(i, j) != 0)
                        cnt++;
            Assert.That(cnt, Is.EqualTo(2));

            gameState = new GameState(size);
            gameState.Set(0, 1, 2);
            gameState.Set(0, 3, 2);
            sut.MoveRight(gameState);

            Assert.That(gameState.Get(0, size - 1), Is.EqualTo(4));
            cnt = 0;
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (gameState.Get(i, j) != 0)
                        cnt++;
            Assert.That(cnt, Is.EqualTo(2));
        }

        [Test]
        public void MoveDownSuccessMerge()
        {
            int size = 4;

            var sut = new GameStateHandler();
            var gameState = new GameState(size);
            gameState.Set(1, 0, 2);
            gameState.Set(0, 0, 2);
            sut.MoveDown(gameState);

            Assert.That(gameState.Get(size - 1, 0), Is.EqualTo(4));
            int cnt = 0;
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (gameState.Get(i, j) != 0)
                        cnt++;
            Assert.That(cnt, Is.EqualTo(2));

            gameState = new GameState(size);
            gameState.Set(2, 0, 2);
            gameState.Set(0, 0, 2);
            sut.MoveDown(gameState);

            Assert.That(gameState.Get(size - 1, 0), Is.EqualTo(4));
            cnt = 0;
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (gameState.Get(i, j) != 0)
                        cnt++;
            Assert.That(cnt, Is.EqualTo(2));

            gameState = new GameState(size);
            gameState.Set(3, 0, 2);
            gameState.Set(0, 0, 2);
            sut.MoveDown(gameState);

            Assert.That(gameState.Get(size - 1, 0), Is.EqualTo(4));
            cnt = 0;
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (gameState.Get(i, j) != 0)
                        cnt++;
            Assert.That(cnt, Is.EqualTo(2));
        }

        [Test]
        public void MultipleMergesNotAmbiguous()
        {
            int size = 4;

            var sut = new GameStateHandler();
            var gameState = new GameState(size);
            gameState.Set(0, 0, 2);
            gameState.Set(0, 1, 2);
            gameState.Set(0, 2, 2);
            sut.MoveLeft(gameState);

            Assert.That(gameState.Get(0, 0), Is.EqualTo(4));
            Assert.That(gameState.Get(0, 1), Is.EqualTo(2));

            gameState = new GameState(size);
            gameState.Set(0, 0, 2);
            gameState.Set(0, 2, 2);
            gameState.Set(0, 3, 2);
            sut.MoveLeft(gameState);

            Assert.That(gameState.Get(0, 0), Is.EqualTo(4));
            Assert.That(gameState.Get(0, 1), Is.EqualTo(2));

            gameState = new GameState(size);
            gameState.Set(0, 0, 2);
            gameState.Set(0, 1, 2);
            gameState.Set(0, 3, 2);
            sut.MoveLeft(gameState);

            Assert.That(gameState.Get(0, 0), Is.EqualTo(4));
            Assert.That(gameState.Get(0, 1), Is.EqualTo(2));
        }

        [Test]
        public void NoDoubleMerge()
        {
            int size = 4;

            var sut = new GameStateHandler();
            var gameState = new GameState(size);
            gameState.Set(0, 0, 2);
            gameState.Set(1, 0, 2);
            gameState.Set(2, 0, 4);
            sut.MoveUp(gameState);

            Assert.That(gameState.Get(0, 0), Is.EqualTo(4));
            Assert.That(gameState.Get(1, 0), Is.EqualTo(4));
        }

        [Test]
        public void NoNewIfNoMerge()
        {

            int size = 4;

            var sut = new GameStateHandler();
            var gameState = new GameState(size);
            gameState.Set(0, 0, 2);
            var result = sut.MoveUp(gameState);

            Assert.That(gameState.Get(0, 0), Is.EqualTo(2));
            Assert.That(result.StateChange, Is.False);
            Assert.That(result.GameOver, Is.False);
        }

        [Test]
        public void NoNewIfFull()
        {
            int size = 4;

            var sut = new GameStateHandler();
            var gameState = new GameState(size);
            gameState.Set(0, 0, 2);
            gameState.Set(0, 1, 4);
            gameState.Set(0, 2, 8);
            gameState.Set(0, 3, 16);
            var result = sut.MoveRight(gameState);

            Assert.That(gameState.Get(0, 0), Is.EqualTo(2));
            Assert.That(gameState.Get(0, 1), Is.EqualTo(4));
            Assert.That(gameState.Get(0, 2), Is.EqualTo(8));
            Assert.That(gameState.Get(0, 3), Is.EqualTo(16));
            Assert.That(result.StateChange, Is.False);
            Assert.That(result.GameOver, Is.False);
        }

        [Test]
        public void TestGameOver()
        {
            int size = 4;

            var sut = new GameStateHandler();
            var gameState = new GameState(size);
            gameState.Set(0, 0, 2);
            gameState.Set(0, 1, 4);
            gameState.Set(0, 2, 8);
            gameState.Set(0, 3, 16);
            gameState.Set(1, 0, 16);
            gameState.Set(1, 1, 8);
            gameState.Set(1, 2, 4);
            gameState.Set(1, 3, 2);
            gameState.Set(2, 0, 2);
            gameState.Set(2, 1, 4);
            gameState.Set(2, 2, 8);
            gameState.Set(2, 3, 16);
            gameState.Set(3, 0, 16);
            gameState.Set(3, 1, 8);
            gameState.Set(3, 2, 4);
            gameState.Set(3, 3, 2);

            var result = sut.MoveRight(gameState);
            Assert.That(result.StateChange, Is.False);
            Assert.That(result.GameOver, Is.True);
            result = sut.MoveLeft(gameState);
            Assert.That(result.StateChange, Is.False);
            Assert.That(result.GameOver, Is.True);
            result = sut.MoveUp(gameState);
            Assert.That(result.StateChange, Is.False);
            Assert.That(result.GameOver, Is.True);
            result = sut.MoveDown(gameState);
            Assert.That(result.StateChange, Is.False);
            Assert.That(result.GameOver, Is.True);
        }

        [Test]
        public void TestScoring()
        {
            int size = 4;

            var sut = new GameStateHandler();
            var gameState = new GameState(size);
            gameState.Set(0, 0, 2);
            gameState.Set(0, 1, 2);
            gameState.Set(0, 2, 0);
            gameState.Set(0, 3, 0);
            var result = sut.MoveLeft(gameState);

            Assert.That(result.StateChange, Is.True);
            Assert.That(result.Score, Is.EqualTo(4));
        }

        [Test]
        public void TestScoringMultiple()
        {
            int size = 4;

            var sut = new GameStateHandler();
            var gameState = new GameState(size);
            gameState.Set(0, 0, 2);
            gameState.Set(0, 1, 2);
            gameState.Set(0, 2, 2);
            gameState.Set(0, 3, 2);
            var result = sut.MoveLeft(gameState);

            Assert.That(result.StateChange, Is.True);
            Assert.That(result.Score, Is.EqualTo(8));
            gameState = new GameState(size);
            gameState.Set(0, 0, 4);
            gameState.Set(0, 1, 4);
            gameState.Set(0, 2, 0);
            gameState.Set(0, 3, 0);
            result = sut.MoveLeft(gameState);
            Assert.That(result.StateChange, Is.True);
            Assert.That(result.Score, Is.EqualTo(8));

            gameState = new GameState(size);
            result = sut.MoveLeft(gameState);
            Assert.That(result.StateChange, Is.False);
            Assert.That(result.Score, Is.EqualTo(0));
        }

    }
}
