using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinefieldGame.Model.Math
{
    public class Point2D
    {
        public int X {  get; set; }
        public int Y { get; set; }

        public Point2D(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }

        public Point ToPoint()
        {
            return new Point(X, Y);
        }

        public Size ToSize()
        {
            return new Size(X, Y);
        }

        public override string ToString()
        {
            return $"Point2D(X={X},Y={Y})";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj == this) return true;

            if (obj is Point2D point)
            {
                return X == point.X && Y == point.Y;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}
