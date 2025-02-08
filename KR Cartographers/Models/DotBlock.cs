using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KR_Cartographers.Models
{
    public class DotBlock : Block
    {
        private readonly Position[][] tiles = new Position[][]
        {
            new Position[]{ new(0,0) }
        };

        public override int Id => 3;
        public override Position StartOffset => new Position(0, 0);
        public override Position[][] Tiles => tiles;
    }
}
