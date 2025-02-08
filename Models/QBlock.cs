using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KR_Cartographers.Models
{
    public class QBlock : Block
    {
        private readonly Position[][] tiles = new Position[][]
        {
            new Position[]{ new(1,2), new(1,3), new(2,0), new(2,1), new(2,2)},
            new Position[]{ new(0,1), new(1,1), new(2,1), new(2,2), new(3,2)},
            new Position[]{ new(1,1), new(1,2), new(1,3), new(2,0), new(2,1)},
            new Position[]{ new(0,1), new(1,1), new(1,2), new(2,2), new(3,2)}
        };

        public override int Id => 4;
        public override Position StartOffset => new Position(0, 0);
        public override Position[][] Tiles => tiles;
    }
}
