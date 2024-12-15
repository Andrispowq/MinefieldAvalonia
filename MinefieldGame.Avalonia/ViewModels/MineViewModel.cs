using Avalonia.Media;
using MinefieldGame.Model.Math;
using MinefieldGame.Model.Mines;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinefieldGame.Avalonia.ViewModels
{
    public class MineViewModel : ViewModelBase
    {
        private Point2D _position;
        private Point2D _gameBounds;

        public Mine Mine { get; init; }

        public double Width => Mine.Size.X;
        public double Height => Mine.Size.Y;
        public Point2D Size => Mine.Size;

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
                    _position = value;
                    OnPropertyChanged();
                }
            }
        }

        public MineViewModel(Mine mine, Point2D gameBounds)
        {
            Mine = mine;
            _position = mine.Position;
            _gameBounds = gameBounds;

            _brush = Brushes.Transparent;
        }

        public bool CheckIfMineEvaded()
        {
            if (Mine.Position.Y >= _gameBounds.Y)
            {
                return true;
            }

            return false;
        }

        public void UpdatePosition()
        {
            Position = Mine.Position;
        }
    }
}
