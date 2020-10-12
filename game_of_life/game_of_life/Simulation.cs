using System;

namespace GameOfLife
{
    /// <summary>
    /// Class that contains logic for the simulation and its proccesses
    /// </summary>
    public class Simulation
    {
        /// <summary>
        /// Count of iterations the simulations has gone through
        /// </summary>
        public int IterationCount { get; set; }

        /// <summary>
        /// Count of cells in simulation that are alive
        /// </summary>
        public int CellCount { get; set; }

        /// <summary>
        /// Amount of rows in simulation's grid
        /// </summary>
        public int Rows { get; set; }

        /// <summary>
        /// Amount of columns in simulation's grid
        /// </summary>
        public int Columns { get; set; }

        /// <summary>
        /// Bool array of simulation's grid
        /// </summary>
        public bool[][] Grid { get; set; }

        /// <summary>
        /// Bool that shows if simulation is active
        /// </summary>
        public bool IsActive { get; set; }

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
            IsActive = true;

            //Initialises grid and populates it with cells at random
            Initialise();
            RandomizeGrid();
        }

        /// <summary>
        /// Initialises simulation
        /// </summary>
        public void Initialise()
        {
            //Initialises grid of the simulation
            Grid = new bool[Rows][];
            for (int i = 0; i < Rows; i++)
            {
                Grid[i] = new bool[Columns];
            }
        }

        /// <summary>
        /// Randomizes simulation's grid
        /// </summary>
        private void RandomizeGrid()
        {
            //For each cell in the grid that is not on the border this will assign
            //a randomised value of either true or false
            Random rand = new Random();
            for (int x = 1; x < Rows - 1; x++)
            {
                for (int y = 1; y < Columns - 1; y++)
                {
                    if (rand.Next(2) > 0)
                    {
                        Grid[x][y] = true;
                        CellCount++;
                    }
                    else
                    {
                        Grid[x][y] = false;
                    }
                }
            }
        }

        /// <summary>
        /// Advances simulation to next iteration acording to the rules of the Game of Life
        /// </summary>
        public void NextIteration()
        {
            bool[][] nextGrid = new bool[Rows][];

            for (int i = 0; i<Rows; i++)
            {
                nextGrid[i] = new bool[Columns];
            }

            for (int x = 1; x < Rows - 1; x++)
            {
                for (int y = 1; y < Columns - 1; y++)
                {
                    nextGrid[x][y] = DoCellAction(x, y);
                    CompareCellStates(Grid[x][y], nextGrid[x][y]);
                }
            }

            Grid = nextGrid;
            IterationCount++;
        }

        /// <summary>
        /// Counts how many cells are alive around cell at given coordinates
        /// </summary>
        /// <param name="x">Cell's x coordinate</param>
        /// <param name="y">Cell's x coordinate</param>
        /// <returns>Count of alive cells around given cell</returns>
        private int CheckNeighborCount(int x, int y)
        {
            int neighborsCount = 0;

            //Check how many alive neighbours cell has
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (Grid[x + i][y + j])
                    {
                        neighborsCount++;
                    }
                }
            }

            //Take away 1 if cell is alive and counted itself
            if (Grid[x][y])
            {
                neighborsCount--;
            }

            return neighborsCount;
        }

        /// <summary>
        /// Checks given cell's neighbour count to determine if cell will be alive or dead
        /// </summary>
        /// <param name="x">Cell's x coordinate</param>
        /// <param name="y">Cell's y coordinate</param>
        /// <returns>Cell's new condition</returns>
        private bool DoCellAction(int x, int y)
        {
            //Find count of neigbhors for current cell
            int neighborsCount = CheckNeighborCount(x, y);

            //Returns true or false depending on neighbour count
            if (neighborsCount == 3)
            {
                return true;
            }
            else if (neighborsCount == 2)
            {
                return Grid[x][y];
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Compares initial and new state of cell to determine if and how to change total cell count in simulation
        /// </summary>
        /// <param name="initialState">Initial state of cell</param>
        /// <param name="newState">New state of cell</param>
        private void CompareCellStates(bool initialState, bool newState)
        {
            if (initialState && !newState)
            {
                CellCount--;
            }
            else if (!initialState && newState)
            {
                CellCount++;
            }
        }
    }
}
