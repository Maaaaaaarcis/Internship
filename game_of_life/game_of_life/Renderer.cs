using System;
using System.Collections.Generic;
using System.Text;

namespace GameOfLife
{
    /// <summary>
    /// Handles rendering functionality to Console for the Game of Life
    /// </summary>
    public class Renderer
    {
        public bool IsPaused { get; set; }

        /// <summary>
        /// Handles rendering functionality to Console for the Game of Life
        /// </summary>
        public Renderer()
        {
            IsPaused = false;
        }

        /// <summary>
        /// Prints starting message to user
        /// </summary>
        public void PrintStartMessage()
        {
            Console.Clear();
            Console.WriteLine("Welcome to the Game of Life!");
        }

        public void AskSimulationInitialisation(out int rows, out int cols, out int simulationCount)
        {
            do
            {
                rows = Input("rows", 20);
                cols = Input("cols", 50);
                simulationCount = Input("simulations", 1);

                Console.WriteLine(
                    "\nAre these inputs correct?" +
                    "\nRows: " + rows +
                    "\nColumns: " + cols +
                    "\nAmount of simulations: " + simulationCount +
                    "\nEnter \"y\" to continue, or enter anything else to go back and change inputs!");
                Console.Clear();
            } while (Console.ReadKey().KeyChar != 'y');
        }

        private int Input(string inputName, int defaultValue)
        {
            int value = defaultValue;
            Console.WriteLine("Please enter the amount of " + inputName + "!");
            if (!Int32.TryParse(Console.ReadLine(), out value))
            {
                Console.WriteLine("Incorrect input, defaulting to " + defaultValue + " " + inputName + "!");
            }
            return value;
        }

        /// <summary>
        /// Prints game statistics to Console
        /// </summary>
        /// <param name="liveGames">Number of simulations that are live</param>
        /// <param name="startingGames">Number of simulations at the start</param>
        /// <param name="cellCount">Number of alive cells in all simulations</param>
        public void StartPrintSimulations(int liveGames, int startingGames, int cellCount)
        {
            Console.Clear();
            if (IsPaused)
            {
                PrintPause();
            }
            Console.WriteLine(
                "Total live games: " + liveGames + "/" + startingGames +
                ", total alive cells: " + cellCount +
                "; press SPACE to pause, press ESC to stop\n");
        }

        /// <summary>
        /// Prints simulation representation to Console
        /// </summary>
        /// <param name="simulation">Simulation formatted as a StringBuilder</param>
        public void PrintSimulation(Simulation simulation)
        {
            //First line is information about simulation properties
            Console.WriteLine("Current iteration: " + simulation.IterationCount +
                "; Count of live cells: " + simulation.CellCount);

            //Next lines in string are a representation of the simulation where '+' are alive cells
            for (int row = 0; row < simulation.Rows; row++)
            {
                for (int col = 0; col < simulation.Columns; col++)
                {
                    if (simulation.Grid[row][col])
                    {
                        Console.Write('+');
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
                Console.Write('\n');
            }
        }

        /// <summary>
        /// Prints reminder that game is paused to Console
        /// </summary>
        private void PrintPause()
        {
            Console.Clear();
            Console.WriteLine("Game is paused!");
            Console.WriteLine("");
        }

        /// <summary>
        /// Asks user if they want to load from a file
        /// </summary>
        /// <returns>File name if user wants to save, null if doesn't</returns>
        public string AskLoad()
        {
            Console.WriteLine("Would you like to load from a file? (y-yes, other-no)");
            if (Console.ReadKey().KeyChar == 'y')
            {
                Console.WriteLine("Please enter the name of the file! (without .txt)");
                return Console.ReadLine();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Asks user if they want to save to file
        /// </summary>
        /// <returns>File name if user wants to save, null if doesn't</returns>
        public string AskSave()
        {
            //Asks user if they want to save current simulation to file
            Console.CursorVisible = true;
            Console.WriteLine("WWould you like to save information to a file? (y-yes, other-no)");
            if (Console.ReadKey().KeyChar == 'y')
            {
                //Asks for file name that information will be saved to
                Console.WriteLine("Please enter a name for this file!");
                return Console.ReadLine();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Asks user if they want to exit program
        /// </summary>
        /// <returns>True if user wants to restart, false if exit</returns>
        public bool AskExit()
        {
            //Asks user if they want to restart the program, if they don't then exits the program loop
            Console.WriteLine("Would you like to restart the program? (Y-yes)");
            string choice = Console.ReadLine();
            if (choice == "Y")
            {
                return true;
            }

            //Final message before exiting program
            Console.WriteLine("\nPress any key to exit program!");
            Console.CursorVisible = false;
            Console.ReadKey();
            return false;
        }
    }
}
