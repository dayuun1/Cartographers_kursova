using KR_Cartographers.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D.Converters;
using System.Xml.Linq;

namespace KR_Cartographers.Models
{
    public class ConditionsCard : Card
    {
        public int CostOfCard { get; }
        public ConditionalCardType conditionalCardType { get; }

        public ConditionsCard(ConditionalCardType conditionalCardType, string name, int costOfCard, string description) : base(name, description)
        {
            this.conditionalCardType = conditionalCardType;
            CostOfCard = costOfCard;
            Description = description;
            Name = Name;
        }

        public int ConditionMethod(GameGrid gameGrid, string name, List<(int, int)> ruinsOnMap)
        {
            int points = 0;
            switch (name)
            {
                case "Borderlands":
                    for (int i = 0; i < gameGrid.Rows; i++)
                    {
                        bool isFull = true;
                        for (int j = 0; j < gameGrid.Columns; j++)
                        {
                            if (!gameGrid.IsCellOccupied(i, j))
                            {
                                isFull = false;
                                break;
                            }
                        }
                        if (isFull)
                        {
                            points += 6;
                        }
                    }
                    for (int j = 0; j < gameGrid.Columns; j++)
                    {
                        bool isFull = true;
                        for (int i = 0; i < gameGrid.Rows; i++)
                        {
                            if (!gameGrid.IsCellOccupied(i, j))
                            {
                                isFull = false;
                                break;
                            }
                        }
                        if (isFull)
                        {
                            points += 6;
                        }
                    }
                    return points;
                case "The Broken Road":
                    for (int i = 0; i < gameGrid.Rows; i++)
                    {
                        bool isFilledDiagonal = true;
                        for (int j = 0; j < gameGrid.Columns; j++)
                        {
                            if (i + j < gameGrid.Rows)
                            {
                                int row = i + j;
                                int col = j;
                                if (!gameGrid.IsCellOccupied(row, col))
                                {
                                    isFilledDiagonal = false;
                                    break;
                                }
                            }
                        }
                        if (isFilledDiagonal)
                        {
                            points += 3;
                        }
                    }
                    return points;
                case "Lost Barony":
                    int maxSquareSize = 0;

                    for (int i = 0; i < gameGrid.Rows; i++)
                    {
                        for (int j = 0; j < gameGrid.Columns; j++)
                        {
                            int currentSquareSize = 1;
                            bool isFilledSquare = true;

                            while (i + currentSquareSize < gameGrid.Rows && j + currentSquareSize < gameGrid.Columns && isFilledSquare)
                            {
                                for (int k = i; k <= i + currentSquareSize; k++)
                                {
                                    for (int l = j; l <= j + currentSquareSize; l++)
                                    {
                                        if (!gameGrid.IsCellOccupied(k, l))
                                        {
                                            isFilledSquare = false;
                                            break;
                                        }
                                    }
                                    if (!isFilledSquare)
                                    {
                                        break;
                                    }
                                }
                                if (isFilledSquare)
                                {
                                    maxSquareSize = Math.Max(maxSquareSize, currentSquareSize + 1);
                                    currentSquareSize++;
                                }
                            }
                        }
                    }
                    points = maxSquareSize > 1 ? maxSquareSize * 3 : 3;
                    return points;
                case "The Cauldrons":
                    int emptyCellsCount = 0;

                    for (int i = 0; i < gameGrid.Rows; i++)
                    {
                        for (int j = 0; j < gameGrid.Columns; j++)
                        {
                            if (gameGrid.IsEmptyOrRuin(i, j))
                            {
                                bool isEmpty = false;
                                if (gameGrid.IsEmptyOrRuin(i, j))
                                {
                                    if (i > 0)
                                    {
                                        if (gameGrid.IsEmptyOrRuin(i - 1, j))
                                        {
                                            isEmpty = true;
                                        }
                                    }

                                    if (i < gameGrid.Rows - 1)
                                    {
                                        if (gameGrid.IsEmptyOrRuin(i + 1, j))
                                        {
                                            isEmpty = true;
                                        }
                                    }

                                    if (j > 0)
                                    {
                                        if (gameGrid.IsEmptyOrRuin(i, j - 1))
                                        {
                                            isEmpty = true;
                                        }
                                    }

                                    if (j < gameGrid.Columns - 1)
                                    {
                                        if (gameGrid.IsEmptyOrRuin(i, j + 1))
                                        {
                                            isEmpty = true;
                                        }
                                    }
                                    if (!isEmpty)
                                    {
                                        emptyCellsCount++;
                                    }
                                }
                            }
                        }
                    }
                        return emptyCellsCount;
                case "Greenbough":
                    int rowsWithForrest = 0;
                    int columnsWithForrest = 0;

                    for (int i = 0; i < gameGrid.Rows; i++)
                    {
                        bool hasForrestInRow = false;
                        for (int j = 0; j < gameGrid.Columns; j++)
                        {
                            if (gameGrid[i, j] == 2)
                            {
                                hasForrestInRow = true;
                                break;
                            }
                        }
                        if (hasForrestInRow)
                        {
                            rowsWithForrest++;
                        }
                    }
                    for (int j = 0; j < gameGrid.Columns; j++)
                    {
                        bool hasForrestInColumn = false;
                        for (int i = 0; i < gameGrid.Rows; i++)
                        {
                            if (gameGrid[i, j] == 2)
                            {
                                hasForrestInColumn = true;
                                break;
                            }
                        }
                        if (hasForrestInColumn)
                        {
                            columnsWithForrest++;
                        }
                    }
                    return columnsWithForrest + rowsWithForrest;
                case "Sentinel Wood":
                    int forrestOnEdgesCount = 0;

                    for (int j = 0; j < gameGrid.Columns; j++)
                    {
                        if (gameGrid[0, j] == 2)
                        {
                            forrestOnEdgesCount++;
                        }
                        if (gameGrid[gameGrid.Rows - 1, j] == 2)
                        {
                            forrestOnEdgesCount++;
                        }
                    }
                    for (int i = 1; i < gameGrid.Rows - 1; i++)
                    {
                        if (gameGrid[i, 0] == 2)
                        {
                            forrestOnEdgesCount++;
                        }
                        if (gameGrid[i, gameGrid.Columns - 1] == 2)
                        {
                            forrestOnEdgesCount++;
                        }
                    }
                    return forrestOnEdgesCount;
                case "Treetower":
                    int surroundedForrestCount = 0;

                    for (int i = 0; i < gameGrid.Rows; i++)
                    {
                        for (int j = 0; j < gameGrid.Columns; j++)
                        {
                            if (gameGrid[i, j] == 2)
                            {
                                bool allNeighborsOccupied = true;
                                if (i > 0)
                                {
                                    if (gameGrid.IsEmptyOrRuin(i - 1, j))
                                    {
                                        allNeighborsOccupied = false;
                                    }
                                }

                                if (i < gameGrid.Rows - 1)
                                {
                                    if (gameGrid.IsEmptyOrRuin(i + 1, j))
                                    {
                                        allNeighborsOccupied = false;
                                    }
                                }

                                if (j > 0)
                                {
                                    if (gameGrid.IsEmptyOrRuin(i, j - 1))
                                    {
                                        allNeighborsOccupied = false;
                                    }
                                }

                                if (j < gameGrid.Columns - 1)
                                {
                                    if (gameGrid.IsEmptyOrRuin(i, j + 1))
                                    {
                                        allNeighborsOccupied = false;
                                    }
                                }

                                if (allNeighborsOccupied)
                                {
                                    surroundedForrestCount++;
                                }
                            }
                        }
                    }
                    return surroundedForrestCount;
                case "Stoneside Forest":

                    bool[,] visited = new bool[gameGrid.Rows, gameGrid.Columns];
                    List<int> connectedMountainsCount = new List<int>();

                    for (int i = 0; i < gameGrid.Rows; i++)
                    {
                        for (int j = 0; j < gameGrid.Columns; j++)
                        {
                            if (gameGrid[i, j] == 6 && !visited[i, j])
                            {
                                int mountainCount = 0;
                                bool hasConnection = false;
                                DFSForMountains(gameGrid, i, j, visited, ref mountainCount, ref hasConnection);
                                if (hasConnection)
                                {
                                    connectedMountainsCount.Add(mountainCount);
                                }
                            }
                        }
                    }

                    return connectedMountainsCount.Sum() * 3;
                case "Canal Lake":
                    int waterAdjacentToFours = 0;
                    int farmAdjacentToThrees = 0;

                    for (int i = 0; i < gameGrid.Rows; i++)
                    {
                        for (int j = 0; j < gameGrid.Columns; j++)
                        {
                            if (gameGrid[i, j] == 3)
                            {
                                if (CheckNeighbors(gameGrid, i, j, 4))
                                {
                                    waterAdjacentToFours++;
                                }
                            }
                            else if (gameGrid[i, j] == 4)
                            {
                                if (CheckNeighbors(gameGrid, i, j, 3))
                                {
                                    farmAdjacentToThrees++;
                                }
                            }
                        }
                    }
                    return waterAdjacentToFours + farmAdjacentToThrees;
                case "Mages Valley":
                    int waterNearMountain = 0;
                    int farmNearMountain = 0;

                    for (int i = 0; i < gameGrid.Rows; i++)
                    {
                        for (int j = 0; j < gameGrid.Columns; j++)
                        {
                            if (gameGrid[i, j] == 3)
                            {
                                if (CheckNeighbors(gameGrid, i, j, 6))
                                {
                                    waterNearMountain++;
                                }
                            }
                            else if (gameGrid[i, j] == 4)
                            {
                                if (CheckNeighbors(gameGrid, i, j, 6))
                                {
                                    farmNearMountain++;
                                }
                            }
                        }
                    }
                    return waterNearMountain * 2 + farmNearMountain;
                case "The Golden Granary":
                    int farmOnRuins = 0;
                    int waterNearRuins = 0;

                    foreach (var (row, col) in ruinsOnMap)
                    {
                        if (gameGrid.IsInside(row, col) && gameGrid[row, col] == 4)
                        {
                            farmOnRuins++;
                        }
                        if (CheckNeighbors(gameGrid, row, col, 3))
                        {
                            if (row - 1 >= 0 && gameGrid.IsCellOccupied(row - 1, col) && gameGrid[row - 1, col] == 3)
                                waterNearRuins++;
                            if (gameGrid.IsCellOccupied(row + 1, col) && gameGrid[row + 1, col] == 3)
                                waterNearRuins++;
                            if (col - 1 >= 0 && gameGrid.IsCellOccupied(row, col - 1) && gameGrid[row, col - 1] == 3)
                                waterNearRuins++;
                            if (gameGrid.IsCellOccupied(row, col + 1) && gameGrid[row, col + 1] == 3)
                                waterNearRuins++;
                        }
                    }
                    return farmOnRuins * 3 + waterNearRuins;
                case "Shoreside Expanse":
                    int groupCount = 0;

                    for (int targetValue = 3; targetValue < 5; targetValue++)
                    {
                        bool[,] visited1 = new bool[gameGrid.Rows, gameGrid.Columns];

                        for (int i = 0; i < gameGrid.Rows; i++)
                        {
                            for (int j = 0; j < gameGrid.Columns; j++)
                            {
                                if (!visited1[i, j] && gameGrid[i, j] == targetValue)
                                {
                                    bool isIsolated = true;
                                    if (DFS(gameGrid, targetValue, i, j, visited1, ref isIsolated) && isIsolated)
                                    {
                                        groupCount++;
                                    }
                                }
                            }
                        }
                    }

                    return groupCount * 3;

                case "Great City":
                    bool[,] visited2 = new bool[gameGrid.Rows, gameGrid.Columns];
                    int largestGroupSize = 0;

                    for (int i = 0; i < gameGrid.Rows; i++)
                    {
                        for (int j = 0; j < gameGrid.Columns; j++)
                        {
                            if (!visited2[i, j] && gameGrid[i, j] == 1)
                            {
                                int groupSize = 0;
                                bool isIsolated = true;
                                DFSForVillageMax(gameGrid, i, j, visited2, ref groupSize, ref isIsolated);
                                if (isIsolated)
                                {
                                    largestGroupSize = Math.Max(largestGroupSize, groupSize);
                                }
                            }
                        }
                    }
                    return largestGroupSize;
                case "Shieldgate":
                    bool[,] visited3 = new bool[gameGrid.Rows, gameGrid.Columns];
                    List<int> groupSizes = new List<int>();

                    for (int i = 0; i < gameGrid.Rows; i++)
                    {
                        for (int j = 0; j < gameGrid.Columns; j++)
                        {
                            if (!visited3[i, j] && gameGrid[i, j] == 1)
                            {
                                int groupSize = 0;
                                DFSForVillageGroup(gameGrid, i, j, visited3, ref groupSize);
                                groupSizes.Add(groupSize);
                            }
                        }
                    }

                    groupSizes.Sort((a, b) => b.CompareTo(a));

                    return groupSizes.Count >= 2 ? groupSizes[1] * 2 : 0;
                case "Withholds":
                    bool[,] visited4 = new bool[gameGrid.Rows, gameGrid.Columns];
                    int largeGroupCount = 0;

                    for (int i = 0; i < gameGrid.Rows; i++)
                    {
                        for (int j = 0; j < gameGrid.Columns; j++)
                        {
                            if (!visited4[i, j] && gameGrid[i, j] == 1)
                            {
                                int groupSize = 0;
                                DFSForVillageGroup(gameGrid, i, j, visited4, ref groupSize);
                                if (groupSize >= 6)
                                {
                                    largeGroupCount++;
                                }
                            }
                        }
                    }

                    return largeGroupCount * 8;
                case "Greengold Plains":
                    bool[,] visited5 = new bool[gameGrid.Rows, gameGrid.Columns];
                    int count = 0;

                    for (int i = 0; i < gameGrid.Rows; i++)
                    {
                        for (int j = 0; j < gameGrid.Columns; j++)
                        {
                            if (!visited5[i, j] && gameGrid[i, j] == 1)
                            {
                                HashSet<int> adjacentNumbers = new HashSet<int>();
                                int groupSize = 0;
                                DFSForThreeAnotherTerrain(gameGrid, i, j, visited5, ref groupSize, adjacentNumbers);

                                if (adjacentNumbers.Count >= 3)
                                {
                                    count++;
                                }
                            }
                        }
                    }

                    return count * 3;
                default:
                    return points;
            }
        }

        private void DFSForMountains(GameGrid gameGrid, int i, int j, bool[,] visited, ref int mountainCount, ref bool hasConnection)
        {
            Stack<(int, int)> stack = new Stack<(int, int)>();
            stack.Push((i, j));
            visited[i, j] = true;
            mountainCount++;

            while (stack.Count > 0)
            {
                (int x, int y) = stack.Pop();

                int[] dRow = { -1, 1, 0, 0 };
                int[] dCol = { 0, 0, -1, 1 };

                for (int d = 0; d < 4; d++)
                {
                    int newRow = x + dRow[d];
                    int newCol = y + dCol[d];

                    if (gameGrid.IsInside(newRow, newCol))
                    {
                        if (gameGrid[newRow, newCol] == 2 && !visited[newRow, newCol])
                        {
                            DFSExplore2(gameGrid, newRow, newCol, visited, ref mountainCount, ref hasConnection);
                        }
                    }
                }
            }
        }

        private void DFSExplore2(GameGrid gameGrid, int i, int j, bool[,] visited, ref int mountainCount, ref bool hasConnection)
        {
            Stack<(int, int)> stack = new Stack<(int, int)>();
            stack.Push((i, j));
            visited[i, j] = true;

            while (stack.Count > 0)
            {
                (int x, int y) = stack.Pop();

                int[] dRow = { -1, 1, 0, 0 };
                int[] dCol = { 0, 0, -1, 1 };

                for (int d = 0; d < 4; d++)
                {
                    int newRow = x + dRow[d];
                    int newCol = y + dCol[d];

                    if (gameGrid.IsInside(newRow, newCol))
                    {
                        if (gameGrid[newRow, newCol] == 6 && !visited[newRow, newCol])
                        {
                            visited[newRow, newCol] = true;
                            mountainCount++;
                            hasConnection = true;
                            stack.Push((newRow, newCol));
                        }
                        else if (gameGrid[newRow, newCol] == 2 && !visited[newRow, newCol])
                        {
                            visited[newRow, newCol] = true;
                            stack.Push((newRow, newCol));
                        }
                    }
                }
            }
        }
        private static bool DFS(GameGrid gameGrid, int targetValue, int i, int j, bool[,] visited, ref bool isIsolated)
        {
            if (!gameGrid.IsInside(i, j) || visited[i, j] || gameGrid[i, j] != targetValue)
            {
                return false;
            }

            visited[i, j] = true;

            int[] dRow = { -1, 1, 0, 0 };
            int[] dCol = { 0, 0, -1, 1 };

            for (int d = 0; d < 4; d++)
            {
                int newRow = i + dRow[d];
                int newCol = j + dCol[d];

                if (!gameGrid.IsInside(newRow, newCol))
                {
                    isIsolated = false;
                }
                else if (gameGrid[newRow, newCol] != targetValue && gameGrid[newRow, newCol] == (targetValue == 3 ? 4 : 3))
                {
                    isIsolated = false;
                }
                else if (gameGrid[newRow, newCol] == targetValue && !visited[newRow, newCol])
                {
                    DFS(gameGrid, targetValue, newRow, newCol, visited, ref isIsolated);
                }
            }

            return true;
        }


        private static void DFSForVillageMax(GameGrid gameGrid, int i, int j, bool[,] visited, ref int groupSize, ref bool isIsolated)
        {
            if (!gameGrid.IsInside(i, j) || visited[i, j] || gameGrid[i, j] != 1)
            {
                return;
            }

            visited[i, j] = true;
            groupSize++;

            int[] dRow = { -1, 1, 0, 0 };
            int[] dCol = { 0, 0, -1, 1 };

            for (int d = 0; d < 4; d++)
            {
                int newRow = i + dRow[d];
                int newCol = j + dCol[d];

                if (gameGrid.IsInside(newRow, newCol))
                {
                    if (gameGrid[newRow, newCol] == 6)
                    {
                        isIsolated = false; // Межує з 6
                    }
                    else if (gameGrid[newRow, newCol] == 1 && !visited[newRow, newCol])
                    {
                        DFSForVillageMax(gameGrid, newRow, newCol, visited, ref groupSize, ref isIsolated);
                    }
                }
            }
        }
        private static void DFSForVillageGroup(GameGrid gameGrid, int i, int j, bool[,] visited, ref int groupSize)
        {
            if (!gameGrid.IsInside(i, j) || visited[i, j] || gameGrid[i, j] != 1)
            {
                return;
            }

            visited[i, j] = true;
            groupSize++;

            int[] dRow = { -1, 1, 0, 0 };
            int[] dCol = { 0, 0, -1, 1 };

            for (int d = 0; d < 4; d++)
            {
                int newRow = i + dRow[d];
                int newCol = j + dCol[d];
                DFSForVillageGroup(gameGrid, newRow, newCol, visited, ref groupSize);
            }
        }

        private static void DFSForThreeAnotherTerrain(GameGrid gameGrid, int i, int j, bool[,] visited, ref int groupSize, HashSet<int> adjacentNumbers)
        {
            if (!gameGrid.IsInside(i, j) || visited[i, j] || gameGrid[i, j] != 1)
            {
                return;
            }

            visited[i, j] = true;
            groupSize++;

            int[] dRow = { -1, 1, 0, 0 };
            int[] dCol = { 0, 0, -1, 1 };

            for (int d = 0; d < 4; d++)
            {
                int newRow = i + dRow[d];
                int newCol = j + dCol[d];

                if (gameGrid.IsInside(newRow, newCol))
                {
                    int cellValue = gameGrid[newRow, newCol];

                    if (cellValue != 1 && cellValue != 0 && cellValue != 7) 
                    {
                        adjacentNumbers.Add(cellValue);
                    }

                    if (cellValue == 1 && !visited[newRow, newCol])
                    {
                        DFSForThreeAnotherTerrain(gameGrid, newRow, newCol, visited, ref groupSize, adjacentNumbers);
                    }
                }
            }
        }

        public bool CheckNeighbors(GameGrid gameGrid, int row, int col, int targetNumber)
        {
            return row - 1 >= 0 && gameGrid.IsCellOccupied(row - 1, col) && gameGrid[row - 1, col] == targetNumber ||
                   gameGrid.IsCellOccupied(row + 1, col) && gameGrid[row + 1, col] == targetNumber ||
                   col - 1 >= 0 && gameGrid.IsCellOccupied(row, col - 1) && gameGrid[row, col - 1] == targetNumber ||
                   gameGrid.IsCellOccupied(row, col + 1) && gameGrid[row, col + 1] == targetNumber;
        }
    }
}

     



