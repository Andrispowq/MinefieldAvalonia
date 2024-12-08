using MinefieldGame.Model.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinefieldGame.Avalonia.ViewModels
{
    public interface IDisplayable
    {
        public Point2D Position { get; set; }
        public Point2D Size { get; set; }
    }
}
