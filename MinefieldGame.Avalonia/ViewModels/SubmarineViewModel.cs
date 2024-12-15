using Avalonia.Animation;
using Avalonia.Media;
using MinefieldGame.Model;
using MinefieldGame.Model.Math;
using MinefieldGame.Model.Mines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinefieldGame.Avalonia.ViewModels
{
    public class SubmarineViewModel : ViewModelBase
    {
        private Point2D _position;
        private Point2D _gameBounds;

        public Submarine Submarine { get; init; }

        public double Width => Submarine.Size.X;
        public double Height => Submarine.Size.Y;
        public Point2D Size => Submarine.Size;

        private IBrush _brush;

        public IBrush Brush
        {
            get => _brush;
            set 
            {
                _brush = value;
                OnPropertyChanged(nameof(Brush));
            }
        }

        public Point2D Position
        {
            get => _position;
            set
            {
                if (_position != value)
                {
                    _position = AdjustPosition(value);
                    OnPropertyChanged();
                }
            }
        }

        public SubmarineViewModel(Submarine submarine, Point2D gameBounds)
        {
            Submarine = submarine;
            _position = submarine.Position;
            _gameBounds = gameBounds;

            _brush = Brushes.Transparent;
        }

        private Point2D AdjustPosition(Point2D point)
        {
            if (point.X < 0) point.X = 0;
            if (point.Y < 0) point.Y = 0;
            if (point.X >= (_gameBounds.X - Width)) point.X = _gameBounds.X - (int)Width;
            if (point.Y >= (_gameBounds.Y - Height)) point.Y = _gameBounds.Y - (int)Height;

            return point;
        }

        public void UpdatePosition()
        {
            Position = Submarine.Position;
        }
    }
}
