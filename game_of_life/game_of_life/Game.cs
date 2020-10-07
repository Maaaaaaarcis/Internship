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
        public bool Play()
        {
            InitialiseGame();
            DoLoop();

            string fileName = Render.AskSave();
            if (fileName != null)
            {
                FileHandler = new FileHandler(fileName);
                FileHandler.Save(Simulations);
            }

            return Render.AskExit();
        }

        private void InitialiseGame()
        {
            Render.PrintStartMessage();

            string loadedFile = Render.AskLoad();
            if (loadedFile != null)
            {
                FileHandler = new FileHandler(loadedFile);
                Simulations = FileHandler.Load();
                InitialiseGame(Simulations);
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

                InitialiseGame(rows, cols);
            }
            LiveGames = SimulationCount;
        }

        private void InitialiseGame(int rows, int cols)
        {
            //Initialises simulations with its constructor, counts total cells
            Simulations = new Simulation[SimulationCount];
            for (int i = 0; i < SimulationCount; i++)
            {
                Simulations[i] = new Simulation(rows, cols);
                TotalAliveCells += Simulations[i].CellCount;
            }
        }

        private void InitialiseGame(Simulation[] simulations)
        {
            SimulationCount = simulations.Length;
            foreach (Simulation simulation in simulations)
            {
                TotalAliveCells += simulation.CellCount;
            }
        }

        private void DoLoop()
        {
            //Writes first iteration to screen and starts timer
            PrintSimulations();
            timer.Start();

            while (true)
            {
                if (Render.KeyIsPressed(ConsoleKey.Spacebar))
                {
                    Render.IsPaused = timer.Enabled;
                    timer.Enabled = !timer.Enabled;
                    if (!timer.Enabled)
                    {
                        PrintSimulations();
                    }
                }

                if (!timer.Enabled && Render.KeyIsPressed(ConsoleKey.Enter))
                {
                    RenderList = Render.AskRenderList(SimulationCount);
                    PrintSimulations();
                }

                if (Render.KeyIsPressed(ConsoleKey.Escape))
                {
                    timer.Stop();
                    Render.IsPaused = false;
                    Render.PrintSimulationMenu(LiveGames, SimulationCount, TotalAliveCells);
                    break;
                }
            }
        }

        private void AdvanceIteration(Object source, ElapsedEventArgs e)
        {
            int oldCellCount;
            foreach (var simulation in Simulations)
            {
                if (simulation.IsActive)
                {
                    LiveGames--;
                    TotalAliveCells -= simulation.CellCount;
                    oldCellCount = simulation.CellCount;
                    
                    simulation.NextIteration();
                    TotalAliveCells += simulation.CellCount;
                    if (simulation.CellCount > 0 && simulation.CellCount != oldCellCount)
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

        private void PrintSimulations()
        {
            Render.PrintSimulationMenu(LiveGames, SimulationCount ,TotalAliveCells);
            for (int i = 0; i < RenderList.Length; i++)
            {
                Render.PrintSimulation(Simulations[RenderList[i]]);
            }
        }
    }
}
