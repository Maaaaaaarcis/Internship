using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace GameOfLife
{
    /// <summary>
    /// Class that contains logic for the simulation and its proccesses
    /// </summary>
    public class Simulation
    {
        /// <summary>
        /// Gets on which iteration the simulation is on
        /// </summary>
        public int IterationCount { get; set; }

        /// <summary>
        /// Gets count of cells in simulation that are alive
        /// </summary>
        public int CellCount { get; set; }

        /// <summary>
        /// Gets amount of rows in simulation's grid
        /// </summary>
        public int Rows { get; set; }

        /// <summary>
        /// Gets amount of columns in simulation's grid
        /// </summary>
        public int Columns { get; set; }

        /// <summary>
        /// Gets bool array of simulation's grid
        /// </summary>
        public bool[,] Grid { get; set; }

        /// <summary>
        /// Class that contains logic for the simulation and its proccesses
        /// </summary>
        public Simulation()
        {

        }

        /// <summary>
        /// Class that contains logic for the simulation and its proccesses
        /// </summary>
        /// <param name="rows">Amount of rows in grid</param>
        /// <param name="cols">Amount of columns in grid</param>
        public Simulation(int rows, int cols)
        {
            //Assigns appropriate values to simulation properties
            IterationCount = 1;
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

        /// <summary>
        /// Class that contains logic for the simulation and its proccesses
        /// </summary>
        /// <param name="json">JSON string that is being loaded</param>
        public Simulation(string json)
        {
            Simulation sim = JsonSerializer.Deserialize<Simulation>(json);
            IterationCount = sim.IterationCount;
            CellCount = sim.CellCount;
            Rows = sim.Rows;
            Columns = sim.Columns;
            Grid = sim.Grid;
        }

        /// <summary>
        /// Cnverts information of the simulation into a stringbuilder so that it can be printed on screen
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Advances simulation to next iteration acording to the rules of the Game of Life
        /// </summary>
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

        /// <summary>
        /// Returns simulation as a JSON document for saving
        /// </summary>
        /// <returns></returns>
        public string ToSaveable()
        {
            string json = JsonSerializer.Serialize(this);
            return json;
        }
    }
}
