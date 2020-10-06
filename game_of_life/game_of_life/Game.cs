using System;
using System.Text.Json;
using System.Timers;

namespace GameOfLife
{
    /// <summary>
    /// Contains logic for the game to run
    /// </summary>
    public class Game
    {
        private int SimulationCount;
        private Simulation[] Simulations;
        private int LiveGames;
        private int TotalAliveCells;
        private Timer timer;
        private Renderer Render;
        private int[] RenderList;
        private FileHandler FileHandler;

        /// <summary>
        /// Contains logic for the game to run
        /// </summary>
        public Game()
        {
            timer = new Timer(1000);
            timer.Elapsed += AdvanceIteration;
            timer.AutoReset = true;
            timer.Enabled = false;
            LiveGames = 0;
            TotalAliveCells = 0;
            Render = new Renderer();
        }

        /// <summary>
        /// Starts the Game of Life, returns true if the user wants to play again
        /// </summary>
        public bool Start()
        {
            Render.PrintStartMessage();

            string loadedFile = Render.AskLoad();
            if (loadedFile != null)
            {
                FileHandler = new FileHandler(loadedFile);
                Simulations = FileHandler.Load();
            }
            else
            {
                int rows, cols;
                Render.AskSimulationInitialisation(out rows, out cols, out SimulationCount);

                //Adds 2 to rows and cols since borders can't be used
                rows += 2;
                cols += 2;

                LiveGames = SimulationCount;

                //Sets first 8 simulations to be rendered if less than 8 total
                if (SimulationCount < 8)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if (i < SimulationCount)
                        {
                            RenderList[i] = i;
                        }
                        else
                        {
                            RenderList[i] = -1;
                        }
                    }
                }                

                //Initialises simulations with its constructor, counts total cells
                Simulations = new Simulation[SimulationCount];
                for (int i = 0; i < SimulationCount; i++)
                {
                    Simulations[i] = new Simulation(rows, cols);
                    TotalAliveCells += Simulations[i].CellCount;
                }
            }

            DoLoop();

            string fileName = Render.AskSave();
            if (fileName != null)
            {
                FileHandler = new FileHandler(fileName);
                FileHandler.Save(Simulations);
            }

            return Render.AskExit();
        }

        private void InitialiseSimulations()
        {

        }

        private void DoLoop()
        {
            //Hides cursor and presents a message to user before starting simulation
            Console.CursorVisible = false;
            Console.WriteLine("Press any key to continue!");
            Console.ReadKey();

            //Writes first iteration to screen and starts timer
            PrintSimulations();
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
                        timer.Enabled = !timer.Enabled;
                        if (!timer.Enabled)
                        {
                            Render.IsPaused = !timer.Enabled;
                            PrintSimulations();
                        }
                    }

                    //If Escape key is pressed, exits the game's loop
                    if (key.Key == ConsoleKey.Escape)
                    {
                        timer.Stop();
                        Console.Clear();
                        Render.StartPrintSimulations(LiveGames, SimulationCount, TotalAliveCells);
                        break;
                    }
                }
            }
        }

        private void AdvanceIteration(Object source, ElapsedEventArgs e)
        {
            foreach (var simulation in Simulations)
            {
                if (simulation.IsActive)
                {
                    LiveGames--;
                    TotalAliveCells -= simulation.CellCount;
                    simulation.NextIteration();
                    if (simulation.CellCount > 0)
                    {
                        LiveGames++;
                        TotalAliveCells += simulation.CellCount;
                    }
                    else
                    {
                        simulation.IsActive = false;
                    }
                }
            }
            PrintSimulations();
        }

        private void PrintSimulations()
        {
            Console.Clear();
            Render.StartPrintSimulations(LiveGames, SimulationCount ,TotalAliveCells);
            for (int i = 0; i < 8 || i < RenderList.Length; i++)
            {
                Render.PrintSimulation(Simulations[RenderList[i]]);
            }
        }
    }
}
