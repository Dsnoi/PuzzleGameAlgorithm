using System;
using System.Collections.Generic;

namespace Test
{
    class Test
    {
        public enum CellStatus
        {
            SCANNED, UNSCANNED
        }
        public class Cell
        {
            private readonly int posX, posY;
            private readonly char color;
            private CellStatus status = CellStatus.UNSCANNED;

            public int X { get { return this.posX; } }
            public int Y { get { return this.posY; } }
            public char Color { get { return this.color; } }
            public CellStatus Status { get { return this.status; } }

            public Cell(char color, int posX, int posY)
            {
                this.color = color;
                this.posX = posX;
                this.posY = posY;
            }

            public void Scanned()
            {
                status = CellStatus.SCANNED;
            }
        }

        public class Puzzle
        {
            private Cell[][] puz;
            private readonly int posXMax, posYMax;
            private Dictionary<Cell, int> dictionary;
            private const int LIMIT = 4;

            public Puzzle(string[] puzzle)
            {
                puz = CreateCells(puzzle);
                posXMax = puz[1].Length - 1;
                posYMax = puz.Length - 1;
                dictionary = new Dictionary<Cell, int>();
                
            }

            public void OutputResultStrings()
            {
                List<Cell> list = GetResult();
                List<char> result = new List<char>();
                foreach (Cell[] cells in puz)
                {
                    result.Clear();
                    foreach (Cell cell in cells)
                    {
                        if (list.IndexOf(cell) >= 0)
                        {
                            result.Add(cell.Color);
                        }
                        else
                        {
                            result.Add(' ');
                        }
                    }
                    Console.WriteLine(String.Join("", result));
                }
            }

            private List<Cell> GetResult()
            {
                List<Cell> cellList = new List<Cell>();
                foreach (Cell[] cells in puz)
                {
                    foreach (Cell cell in cells)
                    {
                        ScanPos(cell);

                        if (dictionary.Count >= LIMIT)
                        {
                            cellList.AddRange(dictionary.Keys);
                        }
                        dictionary.Clear();
                    }
                }
                return cellList;
            }

            private Cell[][] CreateCells(string[] puzzle)
            {
                Cell[][] cells = new Cell[puzzle.Length][];
                for (int i = 0; i < puzzle.Length; i++)
                {
                    string pf = puzzle[i];
                    cells[i] = new Cell[puzzle[i].Length];
                    for (int j = 0; j < pf.Length; j++)
                    {
                        cells[i][j] = new Cell(pf[j], j, i);
                    }
                }
                return cells;
            }

            private void Calculate(Cell nextCell, Cell curCell)
            {
                if (nextCell != null && nextCell.Status == CellStatus.UNSCANNED)
                {
                    if(nextCell.Color == curCell.Color)
                    {
                        nextCell.Scanned();
                        curCell.Scanned();

                        ScanPos(nextCell);

                        dictionary.TryAdd(curCell, 0);
                        dictionary.TryAdd(nextCell, 0);
                    }
                }
            }
            
            private void ScanPos(Cell cell)
            {
                //上
                Cell nextPuz = cell.Y - 1 >= 0 ? puz[cell.Y - 1][cell.X] : null;
                Calculate(nextPuz, cell);

                //下
                nextPuz = cell.Y + 1 <= posYMax ? puz[cell.Y + 1][cell.X] : null;
                Calculate(nextPuz, cell);

                //左
                nextPuz = cell.X - 1 >= 0 ? puz[cell.Y][cell.X - 1] : null;
                Calculate(nextPuz, cell);

                //右
                nextPuz = cell.X + 1 <= posXMax ? puz[cell.Y][cell.X + 1] : null;
                Calculate(nextPuz, cell);
            }
        }

        static void Main(string[] args)
        {
            string[] puzzle =
            {
                "RGGPYR",
                "GRGPYR",
                "RBYBYY"
            };

            Puzzle puz = new Puzzle(puzzle);
            puz.OutputResultStrings();

            Console.ReadKey();
        }
    }
}
