using MinefieldGame.Model.Mines;
using MinefieldGame.Model.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinefieldGame.Tests
{
    [TestClass]
    public class MineTests
    {
        [TestMethod]
        public void TestEasy()
        {
            Mine mine = new EasyMine() { Position = new Point2D(400, 400) };
            
            Assert.AreEqual(mine.MineType, MineType.Easy);
            Assert.AreEqual(mine.Size, new Point2D(30, 30));

            mine.Move(1.0f);

            Assert.AreEqual(mine.Position, new Point2D(400, 501));
        }

        [TestMethod]
        public void TestMedium()
        {
            Mine mine = new MediumMine() { Position = new Point2D(400, 400) };

            Assert.AreEqual(mine.MineType, MineType.Medium);
            Assert.AreEqual(mine.Size, new Point2D(35, 35));

            mine.Move(1.0f);

            Assert.AreEqual(mine.Position, new Point2D(400, 551));
        }

        [TestMethod]
        public void TestHard()
        {
            Mine mine = new HardMine() { Position = new Point2D(400, 400) };

            Assert.AreEqual(mine.MineType, MineType.Hard);
            Assert.AreEqual(mine.Size, new Point2D(40, 40));

            mine.Move(1.0f);

            Assert.AreEqual(mine.Position, new Point2D(400, 601));
        }
    }
}
