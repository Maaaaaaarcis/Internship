using System;
using System.Text;
using System.Threading;

namespace game_of_life
{
    class Program
    {
        static int Rows;
        static int Columns;
        static bool shouldRunSimulation = true;

        static void Main(string[] args)
        {
            // Create grid of simulation
            Cell[,] grid = new Cell[1, 1];
            Console.WriteLine("Do you wish to load a saved file? (Y-yes)");
            string load = Console.ReadLine();
            if (load == "Y")
            {
                Console.WriteLine("Please enter the name of the saved file.");
                string fileName = Console.ReadLine();
                string[] lines = System.IO.File.ReadAllLines(@"./" + fileName + ".txt");
                Rows = Int32.Parse(lines[0].Substring(0, lines[0].IndexOf(',')));
                Columns = Int32.Parse(lines[0].Substring(lines[0].IndexOf(',') + 1));
                grid = new Cell[Rows, Columns];

                for (int x = 0; x < Rows; x++)
                {
                    for (int y = 0; y < Columns; y++)
                    {
                        try
                        {
                            grid[x, y] = new Cell(false);
                            if (lines[x + 1][y] == '+')
                            {
                                grid[x, y].isAlive = true;
                            }
                        }
                        catch { }
                    }
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Enter ammount of rows for simulation:");
                try
                {
                    Rows = Int32.Parse(Console.ReadLine());
                    if (Rows < 2)
                        throw new Exception();
                }
                catch
                {
                    Console.WriteLine("Incorrect input, defaulting to 30 rows.");
                    Rows = 30;
                }

                Console.WriteLine("Enter amount of columns for simulation:");
                try
                {
                    Columns = Int32.Parse(Console.ReadLine());
                    if (Columns < 2)
                        throw new Exception();
                }
                catch
                {
                    Console.WriteLine("Incorrect input, defaulting to 60 columns.");
                    Columns = 60;
                }

                //Initialise randomized grid
                grid = new Cell[Rows, Columns];
                for (int x = 0; x < Rows; x++)
                {
                    for (int y = 0; y < Columns; y++)
                    {
                        grid[x, y] = new Cell(true);
                    }
                }

                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }
            
            // Display grid
            Console.Clear();
            Console.CursorVisible = false;
            if (Rows < 10 || Rows > 50)
                Console.WindowHeight = 35;
            else
                Console.WindowHeight = Rows + 2;
            if (Columns < 90 || Columns > 150)
                Console.WindowWidth = 90;
            else
                Console.WindowWidth = Columns;
            int currentIteration = 1;
            PrintGrid(grid, currentIteration);
            while (shouldRunSimulation)
            {
                //If no key is pressed, continues the game
                if (!Console.KeyAvailable)
                {
                    grid = NextIteration(grid);
                    currentIteration++;
                    PrintGrid(grid, currentIteration);
                }
                //If a key is pressed, checks if it is SPACE or ESC for PAUSE and STOP respectively
                else
                {
                    ConsoleKeyInfo key = Console.ReadKey(false);
                    if (key.Key == ConsoleKey.Spacebar)
                    {
                        do
                        {
                            Console.CursorTop = 1;
                            Console.CursorLeft = 1;
                            Console.Write("CURRENTLY PAUSED");
                            key = Console.ReadKey(false);
                            if (key.Key == ConsoleKey.Escape)
                            {
                                Console.CursorTop = Rows + 2;
                                Console.CursorLeft = 0;
                                break;
                            }
                        } while (key.Key != ConsoleKey.Spacebar);
                    }
                    if (key.Key == ConsoleKey.Escape)
                    {
                        break;
                    }
                }
            }

            Console.WriteLine("WWould you like to save information to a file? (Y-yes)");
            string save = Console.ReadLine();
            if (save == "Y")
            {
                //Create string array to save info
                string[] gridInfo = new string[Rows + 1];
                gridInfo[0] = Rows + "," + Columns;

                //Populates string array with info about grid
                for (int x = 0; x < Rows; x++)
                {
                    for (int y = 0; y < Columns; y++)
                    {
                        if (grid[x, y].isAlive)
                        {
                            gridInfo[x] += '+';
                        }
                        else
                        {
                            gridInfo[x] += ' ';
                        }
                    }
                }

                //Asks for file name and saves it
                Console.WriteLine("Please enter the name for this file:");
                string fileName = Console.ReadLine();
                System.IO.File.WriteAllLines(@"./" + fileName + ".txt", gridInfo);
            }
        }

        private static Cell[,] NextIteration(Cell[,] currentGrid)
        {
            Cell[,] nextGrid = new Cell[Rows, Columns];
            for (int x = 0; x < Rows; x++)
            {
                for (int y = 0; y < Columns; y++)
                {
                    nextGrid[x, y] = new Cell(false);
                }
            }

            //Loop through each cell in grid
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
                            if (currentGrid[x+i, y+j].isAlive)
                            {
                                neighborsCount++;
                            }
                        }
                    }

                    //Checks if cell is alive
                    if (currentGrid[x, y].isAlive)
                    {
                        //Checks alive cell if it is underpopulated or overpopulated, kills it if it is
                        if (neighborsCount < 2 || neighborsCount > 3)
                        {
                            nextGrid[x, y].isAlive = false;
                        }
                        else
                        {
                            nextGrid[x, y].isAlive = true;
                        }
                    }
                    else
                    {
                        //Checks dead cell if it can be reproduced, does so if it can
                        if (neighborsCount > 2)
                        {
                            nextGrid[x, y].isAlive = true;
                        }
                        else
                        {
                            nextGrid[x, y].isAlive = false;
                        }
                    }
                }
            }

            return nextGrid;
        }

        private static void PrintGrid(Cell[,] grid, int iterationCount)
        {
            var printableGrid = new StringBuilder();

            //Count live cells
            int aliveCellCount = 0;
            foreach(Cell cell in grid)
            {
                if (cell.isAlive)
                {
                    aliveCellCount++;
                }
            }

            //Write iteration count and count of live cells
            printableGrid.Append("Current iteration: " + iterationCount +
                "; Count of live cells: " + aliveCellCount +
                "; press SPACE to pause, press ESC to stop\n");

            // Generate printable grid from given grid
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    if (grid[row, col].isAlive)
                    {
                        printableGrid.Append('+');
                    }
                    else
                    {
                        printableGrid.Append(' ');
                    }
                }
                printableGrid.Append('\n');
            }
            Console.Clear();
            //Console.SetCursorPosition(0, 0);
            Console.Write(printableGrid);
            Thread.Sleep(1000);
        }
    }

    class Cell
    {
        public bool isAlive { get; set; }

        public Cell(bool randomize)
        {
            isAlive = false;
            if (randomize)
            {
                Random rand = new Random();
                if (rand.Next(2) > 0)
                {
                    isAlive = true;
                }
                else
                {
                    isAlive = false;
                }
            }
        }
    }
}
