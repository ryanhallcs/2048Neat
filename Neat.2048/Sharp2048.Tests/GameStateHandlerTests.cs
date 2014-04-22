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

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (!(i == 0 && j == 0))
                        Assert.That(gameState.Get(i, j), Is.EqualTo(0));
            Assert.That(gameState.Get(0, 0), Is.EqualTo(1));

            gameState = new GameState(size);
            gameState.Set(0, 2, 1);
            sut.MoveLeft(gameState);

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (!(i == 0 && j == 0))
                        Assert.That(gameState.Get(i, j), Is.EqualTo(0));
            Assert.That(gameState.Get(0, 0), Is.EqualTo(1));

            gameState = new GameState(size);
            gameState.Set(0, 3, 1);
            sut.MoveLeft(gameState);

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (!(i == 0 && j == 0))
                        Assert.That(gameState.Get(i, j), Is.EqualTo(0));
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

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (!(i == 0 && j == size - 1))
                        Assert.That(gameState.Get(i, j), Is.EqualTo(0));
            Assert.That(gameState.Get(0, size - 1), Is.EqualTo(1));

            gameState = new GameState(size);
            gameState.Set(0, 2, 1);
            sut.MoveRight(gameState);

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (!(i == 0 && j == size - 1))
                        Assert.That(gameState.Get(i, j), Is.EqualTo(0));
            Assert.That(gameState.Get(0, size - 1), Is.EqualTo(1));

            gameState = new GameState(size);
            gameState.Set(0, 0, 1);
            sut.MoveRight(gameState);

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (!(i == 0 && j == size - 1))
                        Assert.That(gameState.Get(i, j), Is.EqualTo(0));
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

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (!(i == size - 1 && j == 0))
                        Assert.That(gameState.Get(i, j), Is.EqualTo(0));
            Assert.That(gameState.Get(size - 1, 0), Is.EqualTo(1));

            gameState = new GameState(size);
            gameState.Set(1, 0, 1);
            sut.MoveDown(gameState);

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (!(i == size - 1 && j == 0))
                        Assert.That(gameState.Get(i, j), Is.EqualTo(0));
            Assert.That(gameState.Get(size - 1, 0), Is.EqualTo(1));

            gameState = new GameState(size);
            gameState.Set(2, 0, 1);
            sut.MoveDown(gameState);

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (!(i == size - 1 && j == 0))
                        Assert.That(gameState.Get(i, j), Is.EqualTo(0));
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

        public void NoNewIfNoMerge()
        {

        }
    }
}
