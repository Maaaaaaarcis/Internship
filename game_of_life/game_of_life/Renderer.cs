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
        /// <summary>
        /// Bool that determines wether to print pause menu or not
        /// </summary>
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

        /// <summary>
        /// Asks user to input row, column and simulation count and returns them as out parameters
        /// </summary>
        /// <param name="rows">Row count</param>
        /// <param name="cols">Column count</param>
        /// <param name="simulationCount">Simulation count</param>
        public void AskSimulationInitialisation(out int rows, out int cols, out int simulationCount)
        {
            char repeat;
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

                repeat = Console.ReadKey(true).KeyChar;

                Console.Clear();
            } while (repeat != 'y');
        }

        /// <summary>
        /// Ask user for input
        /// </summary>
        /// <param name="inputName">Name of input asked</param>
        /// <param name="defaultValue">Default value of input that is to be used in case of a failed parse or input under 1</param>
        /// <returns>Value obtained from user</returns>
        private int Input(string inputName, int defaultValue)
        {
            int value;
            Console.WriteLine("\nPlease enter the amount of " + inputName + "!");
            if (!Int32.TryParse(Console.ReadLine(), out value) || value < 1)
            {
                Console.WriteLine("Incorrect input, defaulting to " + defaultValue + " " + inputName + "!");
                value = defaultValue;
            }
            return value;
        }

        /// <summary>
        /// Ask user to input new render list
        /// </summary>
        /// <param name="simulationCount">Number of simulations in play</param>
        /// <returns>Returns new render list</returns>
        public int[] AskRenderList(int simulationCount)
        {
            List<int> renderList;
            do
            {
                //Ask user to input list of renderable simulations
                Console.Clear();
                Console.WriteLine("Which simulations do you wish to be rendered? (divide with comma, up to 8)");
                string input = Console.ReadLine();
                input += ',';

                //Read render list from input string
                renderList = new List<int>();
                string value = "";
                int enteredNumber;
                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i] == ',' && value != "")
                    {
                        enteredNumber = Int32.Parse(value);
                        if (enteredNumber < simulationCount)
                        {
                            renderList.Add(Int32.Parse(value));
                        }
                        value = "";
                    }
                    else
                    {
                        if (Int32.TryParse(input[i] + "", out _))
                        {
                            value += input[i];
                        }
                    }
                }

                if (renderList.Count != 0)
                {
                    //Ask user if render list is correct
                    Console.WriteLine("Program will now render these simulations:");
                    foreach (var item in renderList)
                    {
                        Console.Write(item + "  ");
                    }
                    Console.WriteLine("\nEnter \"y\" to continue, or enter anything else to go back and change inputs!");
                    if (Console.ReadKey(true).KeyChar == 'y')
                    {
                        break;
                    }
                }
            } while (true);

            return renderList.ToArray();
        }

        /// <summary>
        /// Checks to see what, if any, button is pressed by the user
        /// </summary>
        /// <returns>Integer representation of pressed key, or -1 if no key pressed</returns>
        public int CheckKeyPress()
        {
            if (Console.KeyAvailable)
            {
                return (int)Console.ReadKey(true).Key;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Prints game statistics to Console
        /// </summary>
        /// <param name="liveGames">Number of simulations that are live</param>
        /// <param name="startingGames">Number of simulations at the start</param>
        /// <param name="cellCount">Number of alive cells in all simulations</param>
        public void PrintSimulationMenu(int liveGames, int startingGames, int cellCount)
        {
            StringBuilder print = new StringBuilder();

            print.Append(
                "Total live games: " + liveGames + "/" + startingGames +
                ", total alive cells: " + cellCount +
                "; press SPACE to pause, press ESC to stop\n");

            if (IsPaused)
            {
                print.Append(PrintPause());
            }

            Console.Clear();
            Console.Write(print);
        }

        /// <summary>
        /// Prints simulation representation to Console
        /// </summary>
        /// <param name="simulation">Simulation formatted as a StringBuilder</param>
        public void PrintSimulation(Simulation simulation, int index)
        {
            StringBuilder print = new StringBuilder();

            //First line is information about simulation properties
            print.Append("Simulation index: " + index +
                ";Current iteration: " + simulation.IterationCount +
                "; Count of live cells: " + simulation.CellCount +
                "; Status: ");
            if (simulation.IsActive)
            {
                print.Append(" ALIVE\n");
            }
            else
            {
                print.Append(" DEAD\n");
            }

            //Next lines in string are a representation of the simulation where '+' are alive cells
            for (int row = 0; row < simulation.Rows; row++)
            {
                for (int col = 0; col < simulation.Columns; col++)
                {
                    if (simulation.Grid[row][col])
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

            Console.Write(print);
        }

        /// <summary>
        /// Returns string of message that game is paused.
        /// </summary>
        /// <returns>Message that game is paused</returns>
        private string PrintPause()
        {
            return "Game is paused!\n" +
                "If you wish to change which simulations are being rendered, press ENTER!\n";
        }

        /// <summary>
        /// Asks user if they want to load from a file
        /// </summary>
        /// <returns>File name if user wants to save, null if doesn't</returns>
        public string AskLoad()
        {
            Console.WriteLine("Would you like to load from a file? (y-yes, other-no)");
            if (Console.ReadKey(true).KeyChar == 'y')
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
            Console.WriteLine("Would you like to save information to a file? (y-yes, other-no)");
            if (Console.ReadKey(true).KeyChar == 'y')
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
            if (Console.ReadKey(true).KeyChar == 'y')
            {
                return true;
            }

            //Final message before exiting program
            Console.WriteLine("\nPress any key to exit program!");
            Console.ReadKey(true);
            return false;
        }
    }
}
