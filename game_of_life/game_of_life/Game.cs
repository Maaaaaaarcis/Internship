using System;
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
            FileHandler = new FileHandler();
        }

        /// <summary>
        /// Starts the Game of Life
        /// </summary>
        /// <returns>True if wants to play again, false if doesn't</returns>
        public bool Play()
        {
            Initialise();
            DoLoop();

            string fileName = Render.AskSave();
            if (fileName != null)
            {
                FileHandler.SetFileName(fileName);
                FileHandler.Save(Simulations);
            }

            return Render.AskExit();
        }

        /// <summary>
        /// Initialise game depending on user's choice of loading file or not
        /// </summary>
        private void Initialise()
        {
            Render.PrintStartMessage();

            string loadedFile = Render.AskLoad();
            if (loadedFile != null)
            {
                FileHandler.SetFileName(loadedFile);
                Simulations = FileHandler.Load();
                InitialiseSimulations(Simulations);
                RenderList = Render.AskRenderList(Simulations.Length);
            }
            else
            {
                int rows, cols;
                Render.AskSimulationInitialisation(out rows, out cols, out SimulationCount);

                //Adds 2 to rows and cols since borders can't be used
                rows += 2;
                cols += 2;

                RenderList = Render.AskRenderList(SimulationCount);

                InitialiseSimulations(rows, cols);
            }

            LiveGames = SimulationCount;
        }

        /// <summary>
        /// Initialise game with new simulations
        /// </summary>
        /// <param name="rows">Amount of rows for new simulations</param>
        /// <param name="cols">Amount of columns for new simulations</param>
        private void InitialiseSimulations(int rows, int cols)
        {
            //Initialises simulations with its constructor, counts total cells
            Simulations = new Simulation[SimulationCount];
            for (int i = 0; i < SimulationCount; i++)
            {
                Simulations[i] = new Simulation(rows, cols);
                TotalAliveCells += Simulations[i].CellCount;
            }
        }

        /// <summary>
        /// Initialise game with simulations from file
        /// </summary>
        /// <param name="simulations">Simulations from file that game will be initialised with</param>
        private void InitialiseSimulations(Simulation[] simulations)
        {
            SimulationCount = simulations.Length;
            foreach (Simulation simulation in simulations)
            {
                TotalAliveCells += simulation.CellCount;
            }
        }

        /// <summary>
        /// Starts timer for iteration advancement and listens for user key presses for pausing, stopping and opening render list
        /// </summary>
        private void DoLoop()
        {
            //Writes first iteration to screen and starts timer
            PrintSimulations();
            timer.Start();

            while (true)
            {
                switch (Render.CheckKeyPress())
                {
                    case 32:
                        Render.IsPaused = timer.Enabled;
                        timer.Enabled = !timer.Enabled;
                        if (!timer.Enabled)
                        {
                            PrintSimulations();
                        }
                        break;
                    case 13:
                        if (!timer.Enabled)
                        {
                            RenderList = Render.AskRenderList(SimulationCount);
                            PrintSimulations();
                        }
                        break;
                    case 27:
                        timer.Stop();
                        Render.IsPaused = false;
                        Render.PrintSimulationMenu(LiveGames, SimulationCount, TotalAliveCells);
                        return;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Advances iteration for all simulations that are active
        /// </summary>
        /// <param name="source">Source object passed from timer event</param>
        /// <param name="e">Event arguments passed from timer event</param>
        private void AdvanceIteration(Object source, ElapsedEventArgs e)
        {
            foreach (var simulation in Simulations)
            {
                if (simulation.IsActive)
                {
                    LiveGames--;
                    TotalAliveCells -= simulation.CellCount;
                    
                    simulation.NextIteration();
                    TotalAliveCells += simulation.CellCount;
                    if (simulation.CellCount > 0)
                    {
                        LiveGames++;
                    }
                    else
                    {
                        simulation.IsActive = false;
                    }
                }
            }

            PrintSimulations();
        }

        /// <summary>
        /// Calls renderer to print menu and simulations in render list
        /// </summary>
        private void PrintSimulations()
        {
            Render.PrintSimulationMenu(LiveGames, SimulationCount ,TotalAliveCells);
            for (int i = 0; i < RenderList.Length; i++)
            {
                Render.PrintSimulation(Simulations[RenderList[i]], RenderList[i]);
            }
        }
    }
}
