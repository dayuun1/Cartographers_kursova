using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KR_Cartographers.Models
{
    public class Player
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public List<Card> Cards { get; set; }

        public Player(string name) 
        {
            Name = name;
            Points = 0;
        }
    }
}
