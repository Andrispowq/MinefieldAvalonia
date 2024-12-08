using MinefieldGame.Model.Game;
using MinefieldGame.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MinefieldGame.Model.Mines;
using MinefieldGame.Model;
using MinefieldGame.Model.Math;

namespace MinefieldGame.Tests
{
    [TestClass]
    public class GameManagerTests
    {
        [TestMethod]
        public void TestGameManager()
        {
            Mock<IFileManager<GameState>> fileManager = new Mock<IFileManager<GameState>>();
            Mock<IInputHandler> inputHandler = new Mock<IInputHandler>();
            MockTimer timer = new MockTimer();

            GameManager manager = new GameManager(fileManager.Object, inputHandler.Object, timer);
            manager.NewGame();

            Assert.IsNotNull(manager.GameState);
            Assert.AreEqual(manager.GameState.Submarine.MovementSpeed, 200f);
            Assert.AreEqual(manager.GameState.Submarine.Position, new Point2D(400, 400));
            Assert.AreEqual(manager.GameState.Submarine.Size, new Point2D(100, 40));
            Assert.AreEqual(manager.GameState.Mines.Count, 0);
            Assert.AreEqual(manager.GameState.ElapsedSeconds, 0.0, 0.001);

            manager.GameState.Mines.Add(new EasyMine() { Position = new Point2D(0, 0) });
            manager.GameState.Mines.Add(new EasyMine() { Position = new Point2D(400, 400) });

            Assert.AreEqual(manager.GameState.Mines[0].HasCollision(manager.GameState.Submarine), false);
            Assert.AreEqual(manager.GameState.Mines[1].HasCollision(manager.GameState.Submarine), true);

            bool gameEnded = false;
            manager.OnGameEnded += (_, _) => gameEnded = true;

            manager.NewGame();
            manager.GameState.Mines.Add(new EasyMine() { Position = new Point2D(400, 200) });

            //After two seconds, the mine at (400, 200) hits the submarine at (400, 400)
            timer.RaiseTickEvent();
            timer.RaiseTickEvent();

            Assert.IsTrue(gameEnded);
        }
    }
}
