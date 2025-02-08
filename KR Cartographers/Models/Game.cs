using KR_Cartographers.Properties;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;
using System.Xml.Linq;

namespace KR_Cartographers.Models
{
    public static class Game
    {
        public static GameGrid gameGrid { get; private set; }
        public static GameGrid gameGridSecondPlayer { get; private set; }
        public static int Points { get; set; }
        public static int PointsSecondPlayer { get; set; }
        public static int Coins { get; set; }
        public static int CoinsSecondPlayer { get; set; }
        public static int CoinsByMountain { get; set; }
        public static int CoinsByMountainSecondPlayer { get; set; }
        public static int CoinsByBlock { get; set; }
        public static int CoinsByBlockSecondPlayer { get; set; }
        public static List<ConditionsCard> ConditionsList { get; set; }
        public static List<ConditionsCard> CurrentConditionsList { get; set; }
        public static List<Card> CardsList { get; set; }
        public static List<Card> MonstersList { get; set; }
        public static Random rnd = new Random();
        public static List<(int, int)> RuinsFirstMap = new List<(int, int)>
        {
            (1, 5),
            (2, 1),
            (2, 9),
            (8, 1),
            (9, 5),
            (8, 9)
        };

        public static void InitializeGame()
        {
            gameGrid = new GameGrid(11, 11);
            gameGrid[1, 3] = gameGrid[2, 8] = gameGrid[5, 5] = gameGrid[8, 2] = gameGrid[9, 7] = 6;
            gameGrid[1, 5] = gameGrid[2, 1] = gameGrid[2, 9] = gameGrid[8, 1] = gameGrid[9, 5] = gameGrid[8, 9] = 7;
            Points = 0;
            CoinsByMountain = 0;
            CoinsByBlock = 0;
            gameGridSecondPlayer = new GameGrid(11, 11);
            gameGridSecondPlayer[1, 3] = gameGridSecondPlayer[2, 8] = gameGridSecondPlayer[5, 5] = gameGridSecondPlayer[8, 2] = gameGridSecondPlayer[9, 7] = 6;
            gameGridSecondPlayer[1, 5] = gameGridSecondPlayer[2, 1] = gameGridSecondPlayer[2, 9] = gameGridSecondPlayer[8, 1] = gameGridSecondPlayer[9, 5] = gameGridSecondPlayer[8, 9] = 7;
            PointsSecondPlayer = 0;
            CoinsByMountainSecondPlayer = 0;
            CoinsByBlockSecondPlayer = 0;
            List<Card> cards;
            CardsList = new List<Card>();
            cards = new List<Card>
            {
                new Card(new Block[] { new I3Block(), new WBlock() }, new TerrainType[] { TerrainType.Water }, 1, false, false, true, "Great River", "Велика річка"),
                new Card(new Block[] { new I2Block(), new XBlock() }, new TerrainType[] { TerrainType.Farm }, 1, false, false, true, "Farmland", "Фермерські землі"),
                new Card(new Block[] { new VBlock(), new PBlock() }, new TerrainType[] { TerrainType.Village }, 1, false, false, true, "Hamlet", "Фільбарок"),
                new Card(new Block[] { new STilBlock(), new SBlock() }, new TerrainType[] { TerrainType.Forest }, 1, false, false, true, "Forgotten Forest", "Забутий ліс"),
                new Card(new Block[] { new LBlock() }, new TerrainType[] { TerrainType.Farm, TerrainType.Water }, 2, false, false, false, "Hinterland Stream", "Струмок у глушині"),
                new Card(new Block[] { new STBlock() }, new TerrainType[] { TerrainType.Village, TerrainType.Farm }, 2, false, false, false, "Homestead", "Садиба"),
                new Card(new Block[] { new JBlock() }, new TerrainType[] { TerrainType.Forest, TerrainType.Farm }, 2, false, false, false, "Orchard", "Фриктовий сад"),
                new Card(new Block[] { new QBlock() }, new TerrainType[] { TerrainType.Forest, TerrainType.Village }, 2, false, false, false, "Treetop Village", "Село у верховині"),
                new Card(new Block[] { new TBlock() }, new TerrainType[] { TerrainType.Forest, TerrainType.Water }, 2, false, false, false, "Marshlands", "Мочарі"),
                new Card(new Block[] { new IBlock() }, new TerrainType[] { TerrainType.Village, TerrainType.Water }, 2, false, false, false, "Fishing Village", "Рибальське містечко"),
                new Card(new Block[] { new DotBlock() }, new TerrainType[] { TerrainType.Forest, TerrainType.Village, TerrainType.Farm, TerrainType.Water, TerrainType.Monster}, 0, false, false, false, "Rift Lands", "Розломи"),
                new Card(new Block[] {}, new TerrainType[] {}, 0, false, true, false, "Temple Ruins", "Руїни храму"),
                new Card(new Block[] {}, new TerrainType[] {}, 0, false, true, false, "Outpost Ruins", "Руїни форту"),
            };
            Shuffle(cards);
            AddCards(cards);
            MonstersList = new List<Card>();
            cards = new List<Card>
            {
                new Card(new Block[] { new BTilBlock() }, new TerrainType[] { TerrainType.Monster}, 0, true, false, false, "Goblin Attack", "Напад гоблінів"),
                new Card(new Block[] { new IIBlock() }, new TerrainType[] { TerrainType.Monster}, 0, true, false, false, "Bugbear Assault", "Наліт ведмежатників"),
                new Card(new Block[] { new STBlock() }, new TerrainType[] { TerrainType.Monster}, 0, true, false, false, "Kobold Onslaught", "Наступ кобольдів"),
                new Card(new Block[] { new CBlock() }, new TerrainType[] { TerrainType.Monster}, 0, true, false, false, "Gnoll Raid", "Рейд гнолів"),
            };
            Shuffle(cards);
            AddMonsters(cards);
            CardsList.Add(MonstersList[0]);
            ReshuffleCards(CardsList);
            List<ConditionsCard> conCards;
            ConditionsList = new List<ConditionsCard>();
            conCards = new List<ConditionsCard>
            {
                new ConditionsCard(ConditionalCardType.TeretoryCard, "Borderlands", 24, "Прикордоння: отримайте шість зірок репутації за кожен повністю заповнений рядокабо стовпець."),
                new ConditionsCard(ConditionalCardType.TeretoryCard, "Lost Barony", 24, "Втрачене герцогство:отримайте по три зірки репутації за кожну клітинку вздовж одного краю найбільшого квадрата із заповнених клітинок."),
                new ConditionsCard(ConditionalCardType.TeretoryCard, "The Broken Road", 24, "Битий шлях: отримайте по три зірки репутації за кожну завершену діагональну лінію із заповнених клітинок, яка торкається лівого та нижнього країв мапи."),
                new ConditionsCard(ConditionalCardType.TeretoryCard, "The Cauldrons", 20, "Котли: отримайте по одній зірці репутації за кожну порожню клітинку, оточену з усіх чотирьох сторін заповненими клітинками або краєм мапи.")
            };
            Shuffle(conCards);
            AddConditions(ConditionsList, conCards);
            conCards = new List<ConditionsCard>
            {
                new ConditionsCard(ConditionalCardType.VillageCard, "Great City", 16, "Велике місто: отримайте по одній зірці репутації за кожну клітинку містечка в найбільшій групі клітинок містечка, яка не прилягає до клітинок гір."),
                new ConditionsCard(ConditionalCardType.VillageCard, "Greengold Plains", 21, "Край достатку: отримайте по три зірки репутації за кожну групу клітинок містечка, що прилягає до трьох або більше різних типів місцевості."),
                new ConditionsCard(ConditionalCardType.VillageCard, "Shieldgate", 20, "Оборонна брама: отримайте по дві зірки репутації за кожну клітинку містечка в другій за величиною групі клітинок містечка (вона може бути однаковою за розміром із найбільшою групою)."),
                new ConditionsCard(ConditionalCardType.VillageCard, "Withholds", 16, "Дикі землі: отримайте вісім зірок репутації за кожну групу з шести чи більше клітинок містечка.")
            };
            Shuffle(conCards);
            AddConditions(ConditionsList, conCards);
            conCards = new List<ConditionsCard>
            {
                new ConditionsCard(ConditionalCardType.ForrestCard, "Greenbough", 22, "Зелене гілля: отримайте по одній зірці репутації за кожен рядок і стовпець із принаймні однією клітинкою лісу. Та сама клітинка лісу може бути порахована як для рядка, так і для стовпця."),
                new ConditionsCard(ConditionalCardType.ForrestCard, "Sentinel Wood", 25, "Лісова варта: отримайте по одній зірці репутації за кожну клітинку лісу, що прилягає до краю мапи."),
                new ConditionsCard(ConditionalCardType.ForrestCard, "Stoneside Forest", 18, "Гірський ліс: отримайте по три зірки репутації за кожну клітинку гір, з’єднану з іншою клітинкою гір групою клітинок лісу."),
                new ConditionsCard(ConditionalCardType.ForrestCard, "Treetower", 17, "Вежа на дереві: отримайте по одній зірці репутації за кожну клітинку лісу, оточену з усіх чотирьох сторін заповненими клітинками або краєм мапи.")
            };
            Shuffle(conCards);
            AddConditions(ConditionsList, conCards);
            conCards = new List<ConditionsCard>
            {
                new ConditionsCard(ConditionalCardType.WaterFarmCard, "Canal Lake", 24, "Зрошувальний канал: отримайте по одній зірці репутації за кожну клітинку водойми, що прилягає принаймні до однієї клітинки ферми. Отримайте по одній зірці репутації за кожну клітинку ферми, що прилягає принаймні до однієї клітинки водойми."),
                new ConditionsCard(ConditionalCardType.WaterFarmCard, "Mages Valley", 22, "Долина магів: отримайте по дві зірки репутації за кожну клітинку водойми, що прилягає до клітинки гір. Отримайте по одній зірці репутації за кожну клітинку ферми, що прилягає до клітинки гір."),
                new ConditionsCard(ConditionalCardType.WaterFarmCard, "Shoreside Expanse", 27, "Узбережжя: отримайте по три зірки репутації за кожну групу клітинок ферми, що не прилягають до клітинок водойми чи краю мапи. Аналогічно за кожну групу клітинок з водоймами, що не прилягають до клітинок ферми або краю мапи."),
                new ConditionsCard(ConditionalCardType.WaterFarmCard, "The Golden Granary", 20, "Золота комора: отримайте по одній зірці репутації за кожну клітинку водойми, що прилягає до клітинки руїн. Отримайте по три зірки репутації за кожну клітинку ферми поверх клітинки руїн.")
            };
            Shuffle(conCards);
            AddConditions(ConditionsList, conCards);
            CurrentConditionsList = new List<ConditionsCard>();
            conCards = new List<ConditionsCard>
            {
                ConditionsList[0],
                ConditionsList[4],
                ConditionsList[8],
                ConditionsList[12]
            };
            Shuffle(conCards);
            AddConditions(CurrentConditionsList, conCards);

        }

        public static bool BlockIsInside(GameGrid gameGrid, Block CurrentBlock)
        {
            foreach (Position position in CurrentBlock.TilePositions())
            {
                if (!gameGrid.IsInside(position.Row, position.Column))
                {
                    return false;
                }
            }

            return true;
        }

        public static void PlaceBlock(GameGrid gameGrid, Card card, int currentBlock, int currentTerrain)
        {
            foreach (Position position in card.Block[currentBlock].TilePositions())
            {
                gameGrid[position.Row, position.Column] = (int)card.TerrainType[currentTerrain];
            }
        }

        public static bool CanPlaceBlock(GameGrid gameGrid, Block block)
        {
            foreach (Position position in block.TilePositions())
            {
                if (!gameGrid.IsEmptyOrRuin(position.Row, position.Column))
                {
                    return false;
                }
            }
            return true;
        }


        public static bool CanFitBlock(GameGrid gameGrid, Block block, bool isRuin)
        {
            bool isRuinInTile = false;
            for (int i = -1; i < 12; i++)
            {
                for (int j = -1; j < 12; j++)
                {
                    block.Move(i, j);
                    for (int rotation = 0; rotation < block.Tiles.Length; rotation++)
                    {
                        bool shouldBreak = false;
                        foreach (Position position in block.TilePositions())
                        {
                            if (gameGrid.IsRuin(position.Row, position.Column))
                            {
                                isRuinInTile = true;
                            }

                            if (!gameGrid.IsEmptyOrRuin(position.Row, position.Column))
                            {
                                isRuinInTile = false;
                                shouldBreak = true;
                                break;
                            }
                        }
                        if (isRuin)
                        {
                            if (IsRuinInGrid(gameGrid))
                            {
                                if (!isRuinInTile)
                                {
                                    shouldBreak = true;
                                }
                                else
                                {
                                    block.Reset();
                                    return true;
                                }
                            }
                            else
                            {
                                shouldBreak = true;
                            }
                        }
                        block.RotateCW();

                        if (shouldBreak)
                        {
                            continue;
                        }
                        block.Reset();
                        return true;
                    }
                    block.Reset();
                }
            }
            block.Reset();
            return false;
        }



        public static bool IsRuinInGrid(GameGrid gameGrid)
        {
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if (gameGrid[i, j] == 7)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static readonly Dictionary<string, (Position, int, int)> monstersCards = new()
{
    { "Goblin Attack", (new Position( gameGrid.Rows - 3, gameGrid.Columns - 3), -1, -1) },
    { "Bugbear Assault", (new Position(0, gameGrid.Columns - 1), 1, -1) },
    { "Kobold Onslaught", (new Position(gameGrid.Rows - 1, -1), -1, 1) },
    { "Gnoll Raid", (new Position(0, -1), 1, 1) }
};

        private static bool TryPlaceBlock(GameGrid gameGrid, Card card, Position start, int rowStep, int colStep)
        {
            Block block = card.Block[0];

            for (int col = start.Column; col >= 0 && col < gameGrid.Columns; col += colStep)
            {
                for (int row = start.Row; row >= 0 && row < gameGrid.Rows; row += rowStep)
                {
                    block.offset = new Position(row, col);
                    if (CanPlaceBlock(gameGrid, block))
                    {
                        PlaceBlock(gameGrid, card, 0, 0);
                        return true;
                    }
                }
            }
            block.Reset();
            return false;
        }

        public static void AutoDemonsSpawn(GameGrid gameGrid, Card card)
        {
            if (monstersCards.TryGetValue(card.Name, out var place))
            {
                TryPlaceBlock(gameGrid, card, place.Item1, place.Item2, place.Item3);
            }
        }

        public static void RotateBlockCW(GameGrid gameGrid, Block CurrentBlock)
        {
            CurrentBlock.RotateCW();
            if (!BlockIsInside(gameGrid, CurrentBlock))
            {
                CurrentBlock.RotateCCW();
            }
        }

        public static void RotateBlockCW(GameGrid gameGrid, Block CurrentBlock)
        {
            CurrentBlock.RotateCW();
            if (!BlockIsInside(gameGrid, CurrentBlock))
            {
                CurrentBlock.RotateCCW();
            }
        }



        public static void RotateBlockCCW(GameGrid gameGrid, Block CurrentBlock)
        {
            CurrentBlock.RotateCCW();
            if (!BlockIsInside(gameGrid, CurrentBlock))
            {
                CurrentBlock.RotateCW();
            }
        }

        public static void MoveLeft(GameGrid gameGrid, Block CurrentBlock)
        {
            CurrentBlock.Move(0, -1);
            if (!BlockIsInside(gameGrid, CurrentBlock))
            {
                CurrentBlock.Move(0, 1);
            }
        }

        public static void MoveRight(GameGrid gameGrid, Block CurrentBlock)
        {
            CurrentBlock.Move(0, 1);
            if (!BlockIsInside(gameGrid, CurrentBlock))
            {
                CurrentBlock.Move(0, -1);
            }
        }

        public static void MoveDown(GameGrid gameGrid, Block CurrentBlock)
        {
            CurrentBlock.Move(1, 0);
            if (!BlockIsInside(gameGrid, CurrentBlock))
            {
                CurrentBlock.Move(-1, 0);
            }
        }

        public static void MoveUp(GameGrid gameGrid, Block CurrentBlock)
        {
            CurrentBlock.Move(-1, 0);
            if (!BlockIsInside(gameGrid, CurrentBlock))
            {
                CurrentBlock.Move(1, 0);
            }
        }

        public static void Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }

        public static void ReshuffleCards(List<Card> cards)
        {
            Shuffle(cards);
        }

        public static void AddCards(List<Card> lst)
        {
            CardsList.AddRange(lst);
        }

        public static void AddMonsters(List<Card> lst)
        {
            MonstersList.AddRange(lst);
        }

        public static void AddConditions(List<ConditionsCard> conList, List<ConditionsCard> lst)
        {
            conList.AddRange(lst);
        }

        public static int CountConditions(int n, GameGrid gameGrid)
        {
            return CurrentConditionsList[n].ConditionMethod(gameGrid, CurrentConditionsList[n].Name, RuinsFirstMap);
        }

        public static int CountMonsters(GameGrid gameGrid)
        {
            int minusPoints = 0;
            var directions = new (int, int)[] { (-1, 0), (1, 0), (0, -1), (0, 1) };

            for (int i = 0; i < gameGrid.Rows; i++)
            {
                for (int j = 0; j < gameGrid.Columns; j++)
                {
                    if (!gameGrid.IsEmptyOrRuin(i, j)) continue;

                    if (directions.Any(d => gameGrid.IsInside(i + d.Item1, j + d.Item2) && gameGrid[i + d.Item1, j + d.Item2] == 5))
                    {
                        minusPoints--;
                    }
                }
            }
            return minusPoints;
        }

        public static int CountCoinsByMountains(GameGrid gameGrid)
        {
            int countOfMountain = 0;
            var directions = new (int, int)[] { (-1, 0), (1, 0), (0, -1), (0, 1) };

            for (int i = 0; i < gameGrid.Rows; i++)
            {
                for (int j = 0; j < gameGrid.Columns; j++)
                {
                    if (gameGrid[i, j] != 6) continue;

                    if (directions.All(d => !gameGrid.IsInside(i + d.Item1, j + d.Item2) || !gameGrid.IsEmptyOrRuin(i + d.Item1, j + d.Item2)))
                    {
                        countOfMountain++;
                    }
                }
            }
            return countOfMountain;
        }

        public static int CountPoints(int firstLetter, int secondLetter, int coins, int monsters)
        {
            Points += firstLetter + secondLetter + coins + monsters;
            return Points;
        }
        public static int CountPointsSecondPlayer(int firstLetter, int secondLetter, int coins, int monsters)
        {
            PointsSecondPlayer += firstLetter + secondLetter + coins + monsters;
            return PointsSecondPlayer;
        }
    }
}
