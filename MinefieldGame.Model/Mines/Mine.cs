using MinefieldGame.Model.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MinefieldGame.Model.Mines
{
    public enum MineType
    {
        Easy, Medium, Hard
    }

    public abstract record Mine
    {
        public abstract MineType MineType { get; }
        protected abstract float MovementSpeed { get; }
        public required Point2D Position { get; set; }
        public abstract Point2D Size { get; }

        public void Move(float delta)
        {
            float moveY = Position.Y + MovementSpeed * delta;
            int newY = (int)moveY + 1;

            Position = new Point2D(Position.X, newY);
        }

        public bool HasCollision(Submarine submarine)
        {
            int thisLeft = Position.X;
            int thisRight = Position.X + Size.X;
            int thisTop = Position.Y;
            int thisBottom = Position.Y + Size.Y;

            int otherLeft = submarine.Position.X;
            int otherRight = submarine.Position.X + submarine.Size.X;
            int otherTop = submarine.Position.Y;
            int otherBottom = submarine.Position.Y + submarine.Size.Y;

            return !(thisRight <= otherLeft || thisLeft >= otherRight || thisBottom <= otherTop || thisTop >= otherBottom);
        }
    }
}
