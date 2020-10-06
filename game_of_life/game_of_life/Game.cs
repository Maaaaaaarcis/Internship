using System;
using System.Timers;

namespace GameOfLife
{
    /// <summary>
    /// Contains logic for the game to run
    /// </summary>
    public class Game
    {
        private Simulation simulation { get; set; }
        private Timer timer { get; set; }

        /// <summary>
        /// Contains logic for the game to run
        /// </summary>
        public Game()
        {
            timer = new Timer(1000);
            timer.Elapsed += AdvanceIteration;
            timer.AutoReset = true;
            timer.Enabled = false;
        }

        /// <summary>
        /// Starts the Game of Life, returns true if the user wants to play again
        /// </summary>
        public bool Start()
        {
            Console.Clear();
            Console.WriteLine("Welcome to the Game of Life!\n" +
                "Would you like to load from a file? (Y-yes, other-no)");
            string load = Console.ReadLine();

            if (load == "Y")
            {
                Console.WriteLine("Please enter the name of the file! (without .txt)");
                string fileName = Console.ReadLine();

                FileHandler fileHandler = new FileHandler(fileName);
                simulation = new Simulation(fileHandler.Load());
            }
            else
            {
                int rows;
                int cols;
                string cont;
                do
                {
                    //If user enters a non-number or number below 3, then will set default values to rows and cols
                    Console.WriteLine("\nPlease enter the amount of rows for simulation!");
                    try
                    {
                        rows = Int32.Parse(Console.ReadLine());
                        if (rows < 3)
                            throw new Exception();
                    }
                    catch
                    {
                        Console.WriteLine("Error in input, defaulting to 30 rows.");
                        rows = 30;
                    }

                    Console.WriteLine("Please enter the amount of columns for simulation!");
                    try
                    {
                        cols = Int32.Parse(Console.ReadLine());
                        if (cols < 3)
                            throw new Exception();
                    }
                    catch
                    {
                        Console.WriteLine("Error in input, defaulting to 60 columns.");
                        cols = 60;
                    }

                    //Asks user if inputs are correct, if user decides they aren't then they can reinput them
                    Console.WriteLine("\nAre these inputs correct?\n" +
                        "Rows: " + rows + "\nColumns: " + cols +
                        "\nEnter \"Y\" to continue, or enter anything else to go back and change inputs!");
                    cont = Console.ReadLine();
                    Console.Clear();
                } while (cont != "Y");

                //Adds 2 to rows and cols since borders can't be used
                rows += 2;
                cols += 2;

                //Initialises simulation with its constructor
                simulation = new Simulation(rows, cols);
            }

            //Hides cursor and presents a message to user before starting simulation
            Console.CursorVisible = false;
            Console.WriteLine("Press any key to continue!");
            Console.ReadKey();

            //Writes first iteration to screen and starts timer
            Console.Clear();
            Console.Write(simulation.ToStringBuilder());
            timer.Start();

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    //If a key is pressed, checks what that key is
                    ConsoleKeyInfo key = Console.ReadKey(false);

                    //If it is spacebar, toggles the timer
                    if (key.Key == ConsoleKey.Spacebar)
                    {
                        Console.CursorTop = 1;
                        Console.CursorLeft = 1;
                        Console.Write("SIMULATION PAUSED");
                        timer.Enabled = !timer.Enabled;
                    }

                    //If Escape key is pressed, exits the game's loop
                    if (key.Key == ConsoleKey.Escape)
                    {
                        timer.Stop();
                        Console.CursorLeft = 0;
                        Console.CursorTop = simulation.Rows + 2;
                        break;
                    }
                }
            }

            //Asks user if they want to save current simulation to file
            Console.CursorVisible = true;
            Console.WriteLine("WWould you like to save information to a file? (Y-yes)");
            string choice = Console.ReadLine();
            if (choice == "Y")
            {
                //Asks for file name that information will be saved to
                Console.WriteLine("Please enter a name for this file!");
                string fileName = Console.ReadLine();

                //Saves information about simulation from prior string array to requested file name
                FileHandler fileHandler = new FileHandler(fileName);
                fileHandler.Save(simulation.ToSaveable());
            }

            //Asks user if they want to restart the program, if they don't then exits the program loop
            Console.WriteLine("Would you like to restart the program? (Y-yes)");
            choice = Console.ReadLine();
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

        private void AdvanceIteration(Object source, ElapsedEventArgs e)
        {
            simulation.NextIteration();
            Console.Clear();
            Console.Write(simulation.ToStringBuilder());
        }
    }
}
