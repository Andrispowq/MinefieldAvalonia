using MinefieldGame.Model.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MinefieldGame.Model
{
    public record Submarine
    {
        public required Point2D Position { get; set; }
        public required Point2D Size { get; set; }
        public float MovementSpeed { get; set; }

        public void Move(MoveDirection dir, float delta)
        {
            (float x, float y) = dir switch
            {
                MoveDirection.Left => (-delta * MovementSpeed, 0f),
                MoveDirection.Right => (delta * MovementSpeed, 0f),
                MoveDirection.Up => (0f, -delta * MovementSpeed),
                MoveDirection.Down => (0f, delta * MovementSpeed),
                _ => (0f, 0f)
            };

            (int dx, int dy) = ((int)x, (int)y);

            // Every movement has +1 by default because the int conversion would not allow small movements
            if (x > 0.0) dx++;
            else if (x < 0.0) dx--;

            if (y > 0.0) dy++;
            else if (y < 0.0) dy--;

            Position = new Point2D(Position.X + dx, Position.Y + dy);
        }
    }
}
