using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace KR_Cartographers.Models
{
    public class Card
    {
        public Block[] Block { get; protected set; }
        public TerrainType[] TerrainType { get; protected set; }
        public byte Days { get; protected set; }
        public bool IsMonster {  get; protected set; }
        public bool IsRuin { get; protected set; }
        public bool IsBlockOriental { get; protected set; }
        public string Name { get; protected set; }
        public string Description {  get; protected set; }

        public Card() { }

        public Card(Block[] block, TerrainType[] terrainType, byte days, bool isMonster, bool isRuin, bool isBlockOriental, string name, string description)
        {
            Block = block;
            TerrainType = terrainType;
            Days = days;
            IsMonster = isMonster;
            IsRuin = isRuin;
            IsBlockOriental = isBlockOriental;
            Name = name;
            Description = description;
        }
        public Card(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
