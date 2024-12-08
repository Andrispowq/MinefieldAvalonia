using MinefieldGame.Model;
using MinefieldGame.Model.Math;

namespace MinefieldGame.Tests
{
    [TestClass]
    public class SubmarineTests
    {
        [TestMethod]
        public void TestMove()
        {
            Submarine submarine = new Submarine()
            {
                Position = new Point2D(400, 400),
                Size = new Point2D(40, 40),
                MovementSpeed = 150.0f
            };

            submarine.Move(MoveDirection.Up, 1f);
            Assert.AreEqual(submarine.Position, new Point2D(400, 249));

            submarine.Move(MoveDirection.Right, 1f);
            Assert.AreEqual(submarine.Position, new Point2D(551, 249));

            submarine.Move(MoveDirection.Down, 1f);
            Assert.AreEqual(submarine.Position, new Point2D(551, 400));

            submarine.Move(MoveDirection.Left, 1f);
            Assert.AreEqual(submarine.Position, new Point2D(400, 400));

            Assert.AreEqual(submarine.Size, new Point2D(40, 40));
            Assert.AreEqual(submarine.MovementSpeed, 150.0f);
        }
    }
}