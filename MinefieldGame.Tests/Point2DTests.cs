using MinefieldGame.Model.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinefieldGame.Tests
{
    [TestClass]
    public class Point2DTests
    {
        [TestMethod]
        public void TestPoint()
        {
            Point2D point = new Point2D(400, 400);
            Point2D point2 = new Point2D(400, 400);
            Point2D point3 = new Point2D(500, 400);

            Assert.AreEqual(point.X, 400);
            Assert.AreEqual(point.Y, 400);

            Assert.AreEqual(point.Equals(point2), true);
            Assert.AreEqual(point.Equals(point3), false);

            Assert.AreEqual(point.ToSize(), new System.Drawing.Size(400, 400));
            Assert.AreEqual(point.ToPoint(), new System.Drawing.Point(400, 400));
        }
    }
}
