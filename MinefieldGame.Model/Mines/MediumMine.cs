using MinefieldGame.Model.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinefieldGame.Model.Mines
{
    public record MediumMine : Mine
    {
        public override MineType MineType => MineType.Medium;
        protected override float MovementSpeed => 150.0f;
        public override Point2D Size => new Point2D(35, 35);
    }
}
