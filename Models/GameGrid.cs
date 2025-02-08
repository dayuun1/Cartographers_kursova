using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KR_Cartographers.Models
{
    public class GameGrid
    {
        public readonly int[,] grid;
        public int Rows { get; }
        public int Columns { get; }

        public int this[int rows, int columns]
        {
            get => grid[rows, columns];
            set => grid[rows, columns] = value;
        }

        public GameGrid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            grid = new int[rows, columns];
        }

        public bool IsInside(int rows, int columns)
        {
            return rows >= 0 && rows < Rows && columns >= 0 && columns < Columns;
        }

        public bool IsEmpty(int rows, int columns)
        {
            return IsInside(rows, columns) && grid[rows, columns] == 0;
        }

        public bool IsEmptyOrRuin(int rows, int columns)
        {
            return IsInside(rows, columns) && (grid[rows, columns] == 0 || grid[rows, columns] == 7);
        }

        public bool IsRuin(int rows, int columns)
        {
            return IsInside(rows, columns) && grid[rows, columns] == 7;
        }

        public bool IsCellOccupied(int row, int column)
        {
            return IsInside(row, column) && (grid[row, column] == 1 || grid[row, column] == 2 || grid[row, column] == 3 || grid[row, column] == 4 || grid[row, column] == 5 || grid[row, column] == 6) ;
        }

        public bool IsEmpty(int rows, int columns, GameGrid gameGrid)
        {
            return IsInside(rows, columns) && gameGrid[rows, columns] == 0;
        }

        public bool IsEmptyOrRuin(int rows, int columns, GameGrid gameGrid)
        {
            return IsInside(rows, columns) && (gameGrid[rows, columns] == 0 || gameGrid[rows, columns] == 7);
        }

        public bool IsRuin(int rows, int columns, GameGrid gameGrid)
        {
            return IsInside(rows, columns) && gameGrid[rows, columns] == 7;
        }

        public bool IsCellOccupied(int row, int column, GameGrid gameGrid)
        {
            return IsInside(row, column) && (gameGrid[row, column] == 1 || gameGrid[row, column] == 2 || gameGrid[row, column] == 3 || gameGrid[row, column] == 4 || gameGrid[row, column] == 5 || gameGrid[row, column] == 6);
        }
    }
}