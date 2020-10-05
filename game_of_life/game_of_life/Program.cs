using System;
using System.Text;
using System.Threading;

namespace game_of_life
{
    class Program
    {
        static void Main(string[] args)
        {
            int rows = 3;
            int cols = 3;
            Simulation simulation;
            bool shouldRunSimulation = true;

            Console.WriteLine("Welcome to the Game of Life!\n" +
                "Would you like to load from a file? (Y-yes, other-no)");
            string load = Console.ReadLine();

            if (load == "Y")
            {
                Console.WriteLine("Please enter the name of the file! (without .txt)");
                string fileName = Console.ReadLine();

                string[] lines = System.IO.File.ReadAllLines(@"./" + fileName + ".txt");

                simulation = new Simulation(lines);
            }
            else
            {
                string cont = "";
                do
                {
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

                    Console.WriteLine("\nAre these inputs correct?\n" +
                        "Rows: " + rows + "\nColumns: " + cols +
                        "\nEnter \"Y\" to continue, or enter anything else to go back and change inputs!");
                    cont = Console.ReadLine();
                    Console.Clear();
                } while (cont != "Y");

                rows += 2;
                cols += 2;
                simulation = new Simulation(rows, cols);
            }

            Console.WriteLine("Press any key to continue!");
            Console.ReadKey();
            Console.CursorVisible = false;

            Console.Clear();
            Console.Write(simulation.ToStringBuilder());

            while (shouldRunSimulation)
            {
                Thread.Sleep(1000);
                if (!Console.KeyAvailable)
                {
                    simulation.NextIteration();
                    Console.Clear();
                    Console.Write(simulation.ToStringBuilder());
                }
                else
                {
                    ConsoleKeyInfo key = Console.ReadKey(false);
                    if (key.Key == ConsoleKey.Spacebar)
                    {
                        do
                        {
                            Console.CursorTop = 1;
                            Console.CursorLeft = 1;
                            Console.Write("SIMULATION PAUSED");
                            key = Console.ReadKey(false);
                            if (key.Key == ConsoleKey.Escape)
                            {
                                Console.CursorTop = rows + 2;
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

            Console.CursorVisible = true;
            Console.WriteLine("WWould you like to save information to a file? (Y-yes)");
            string save = Console.ReadLine();
            if (save == "Y")
            {
                string[] gridInfo = simulation.ToSaveable();

                Console.WriteLine("Please enter a name for this file!");
                string fileName = Console.ReadLine();
                System.IO.File.WriteAllLines(@"./" + fileName + ".txt", gridInfo);
            }
        }
    }
}
