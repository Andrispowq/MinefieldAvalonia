using MinefieldGame.Model.Math;
using MinefieldGame.Avalonia.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls.Shapes;
using Avalonia.Controls;

namespace MinefieldGame.Avalonia
{
    internal class RectangleDisplay : IDisplayable
    {
        private Point2D _position;
        private Point2D _size;

        public Rectangle Rectangle { get; private set; }

        public RectangleDisplay(Point2D position, Point2D size, Rectangle rectangle)
        {
            _position = position;
            _size = size;
            Rectangle = rectangle;
        }

        public Point2D Position 
        { 
            get => _position;
            set
            {
                _position = value;
                Canvas.SetLeft(Rectangle, _position.X);
                Canvas.SetTop(Rectangle, _position.Y);
            }
        }

        public Point2D Size 
        {
            get => _size;
            set
            {
                _size = value;
                Canvas.SetRight(Rectangle, _position.X + _size.X);
                Canvas.SetBottom(Rectangle, _position.Y + _size.Y);
            }
        }
    }
}
