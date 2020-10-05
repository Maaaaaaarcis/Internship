using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace game_of_life
{
    class Simulation
    {
        public int IterationCount { get; private set; }
        public int CellCount { get; private set; }
        public int Rows { get; private set; }
        public int Columns { get; private set; }
        private bool[,] Grid { get; set; }

        public Simulation(int rows, int cols)
        {
            IterationCount = 0;
            CellCount = 0;
            Rows = rows;
            Columns = cols;
            GenerateRandom();
        }

        public Simulation(string[] fileGrid)
        {
            IterationCount = Int32.Parse(fileGrid[0].Substring(0, fileGrid[0].IndexOf(',')));
            CellCount = Int32.Parse(fileGrid[0].Substring(fileGrid[0].IndexOf(',') + 1));
            Rows = Int32.Parse(fileGrid[1].Substring(0, fileGrid[1].IndexOf(',')));
            Columns = Int32.Parse(fileGrid[1].Substring(fileGrid[1].IndexOf(',') + 1));

            bool[,] grid = new bool[Rows, Columns];
            for (int x = 0; x < Rows; x++)
            {
                for (int y = 0; y < Columns; y++)
                {
                    try
                    {
                        if (fileGrid[x + 2][y] == '+')
                        {
                            grid[x, y] = true;
                        }
                        else
                        {
                            grid[x, y] = false;
                        }
                    }
                    catch { }
                }
            }
            Grid = grid;
        }

        private void GenerateRandom()
        {
            Grid = new bool[Rows, Columns];
            for (int x = 1; x < Rows - 1; x++)
            {
                for (int y = 1; y < Columns - 1; y++)
                {
                    Random rand = new Random();
                    if (rand.Next(2) > 0)
                    {
                        Grid[x, y] = true;
                        CellCount++;
                    }
                    else
                    {
                        Grid[x, y] = false;
                    }
                }
            }
        }

        public StringBuilder ToStringBuilder()
        {
            StringBuilder print = new StringBuilder();

            print.Append("Current iteration: " + IterationCount +
                "; Count of live cells: " + CellCount +
                "; press SPACE to pause, press ESC to stop\n");

            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    if (Grid[row, col])
                    {
                        print.Append('+');
                    }
                    else
                    {
                        print.Append(' ');
                    }
                }
                print.Append('\n');
            }

            return print;
        }

        public void NextIteration()
        {
            bool[,] nextGrid = new bool[Rows, Columns];
            IterationCount++;

            for (int x = 1; x < Rows - 1; x++)
            {
                for (int y = 1; y < Columns - 1; y++)
                {
                    //Find count of neigbhors for current cell
                    int neighborsCount = -1;
                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            if (Grid[x + i, y + j])
                            {
                                neighborsCount++;
                            }
                        }
                    }

                    //Checks if cell is alive
                    if (Grid[x, y])
                    {
                        //Checks alive cell if it is underpopulated or overpopulated, kills it if it is
                        if (neighborsCount < 2 || neighborsCount > 3)
                        {
                            nextGrid[x, y] = false;
                            CellCount--;
                        }
                        else
                        {
                            nextGrid[x, y] = true;
                        }
                    }
                    else
                    {
                        //Checks dead cell if it can be reproduced, does so if it can
                        if (neighborsCount > 2)
                        {
                            nextGrid[x, y] = true;
                            CellCount++;
                        }
                        else
                        {
                            nextGrid[x, y] = false;
                        }
                    }
                }
            }

            Grid = nextGrid;
        }

        public string[] ToSaveable()
        {
            string[] lines = new string[Rows + 2];

            lines[0] = IterationCount + "," + CellCount;
            lines[1] = Rows + "," + Columns;

            for (int x = 0; x < Rows; x++)
            {
                for (int y = 0; y < Columns; y++)
                {
                    if (Grid[x, y])
                    {
                        lines[x + 2] += '+';
                    }
                    else
                    {
                        lines[x + 2] += ' ';
                    }
                }
            }

            return lines;
        }
    }
}
