using MinefieldGame.Model.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinefieldGame.Model.Mines
{
    public record HardMine : Mine
    {
        public override MineType MineType => MineType.Hard;
        protected override float MovementSpeed => 200.0f;
        public override Point2D Size => new Point2D(40, 40);
    }
}
