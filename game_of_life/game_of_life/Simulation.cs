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
            //Assigns appropriate values to simulation properties
            IterationCount = 0;
            CellCount = 0;
            Rows = rows;
            Columns = cols;

            //Initialises grid of the simulation
            Grid = new bool[Rows, Columns];

            //For each cell in the grid that is not on the border this will assign
            //a randomised value of either true or false
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

        public Simulation(string[] fileGrid)
        {
            //Reads first 2 lines that contain property values and assigns them to current simulation
            IterationCount = Int32.Parse(fileGrid[0].Substring(0, fileGrid[0].IndexOf(',')));
            CellCount = Int32.Parse(fileGrid[0].Substring(fileGrid[0].IndexOf(',') + 1));
            Rows = Int32.Parse(fileGrid[1].Substring(0, fileGrid[1].IndexOf(',')));
            Columns = Int32.Parse(fileGrid[1].Substring(fileGrid[1].IndexOf(',') + 1));

            //Initialises grid of the simulation
            Grid = new bool[Rows, Columns];

            //Reads file's next lines for cell occupancy
            for (int x = 0; x < Rows; x++)
            {
                for (int y = 0; y < Columns; y++)
                {
                    try
                    {
                        if (fileGrid[x + 2][y] == '+')
                        {
                            Grid[x, y] = true;
                        }
                        else
                        {
                            Grid[x, y] = false;
                        }
                    }
                    catch { }
                }
            }
        }

        public StringBuilder ToStringBuilder()
        {
            //StringBuilder that will be returned and contains info about simulation
            StringBuilder print = new StringBuilder();

            //First line is information about simulation properties
            print.Append("Current iteration: " + IterationCount +
                "; Count of live cells: " + CellCount +
                "; press SPACE to pause, press ESC to stop\n");

            //Next lines in string are a representation of the simulation where '+' are alive cells
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
            //String array that will have information about simulation
            string[] lines = new string[Rows + 2];

            //Writes first 2 lines with information about simulation properties
            lines[0] = IterationCount + "," + CellCount;
            lines[1] = Rows + "," + Columns;

            //Next writes the grid of the simulation to file
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
