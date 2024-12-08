using MinefieldGame.Model.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinefieldGame.Model.Mines
{
    public record EasyMine : Mine
    {
        public override MineType MineType => MineType.Easy;
        protected override float MovementSpeed => 100.0f;
        public override Point2D Size => new Point2D(30, 30);
    }
}
