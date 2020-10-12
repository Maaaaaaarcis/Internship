using System;
using System.Collections.Generic;
using System.Timers;

namespace GameOfLife
{
    // <summary>
    /// Contains logic for the game to run
    /// </summary>
    public class Game
    {
        private int simulationCount;
        private Simulation[] simulations;
        private int liveGames;
        private int totalAliveCells;
        private Timer timer;
        private Renderer render;
        private List<int> renderList;
        private FileHandler fileHandler;

        /// <summary>
        /// Contains logic for the game to run
        /// </summary>
        public Game()
        {
            timer = new Timer(1000);
            timer.Elapsed += AdvanceIteration;
            timer.AutoReset = true;
            timer.Enabled = false;
            liveGames = 0;
            totalAliveCells = 0;
            render = new Renderer();
            fileHandler = new FileHandler();
        }

        /// <summary>
        /// Starts the Game of Life
        /// </summary>
        /// <returns>True if wants to play again, false if doesn't</returns>
        public bool Play()
        {
            Initialise();
            DoLoop();

            string fileName = render.AskSave();
            if (fileName != null)
            {
                fileHandler.SetFileName(fileName);
                fileHandler.Save(simulations);
            }

            return render.AskExit();
        }

        /// <summary>
        /// Initialise game depending on user's choice of loading file or not
        /// </summary>
        private void Initialise()
        {
            render.PrintStartMessage();

            string loadedFile = render.AskLoad();
            if (loadedFile != null)
            {
                fileHandler.SetFileName(loadedFile);
                simulations = fileHandler.Load();
                InitialiseSimulations(simulations);
                renderList = render.AskRenderList(simulations.Length);
            }
            else
            {
                int rows, cols;
                render.AskSimulationInitialisation(out rows, out cols, out simulationCount);

                //Adds 2 to rows and cols since borders can't be used
                rows += 2;
                cols += 2;

                renderList = render.AskRenderList(simulationCount);

                InitialiseSimulations(rows, cols);
            }

            liveGames = simulationCount;
        }

        /// <summary>
        /// Initialise game with new simulations
        /// </summary>
        /// <param name="rows">Amount of rows for new simulations</param>
        /// <param name="cols">Amount of columns for new simulations</param>
        private void InitialiseSimulations(int rows, int cols)
        {
            //Initialises simulations with its constructor, counts total cells
            simulations = new Simulation[simulationCount];
            for (int i = 0; i < simulationCount; i++)
            {
                simulations[i] = new Simulation(rows, cols);
                totalAliveCells += simulations[i].CellCount;
            }
        }

        /// <summary>
        /// Initialise game with simulations from file
        /// </summary>
        /// <param name="simulations">Simulations from file that game will be initialised with</param>
        private void InitialiseSimulations(Simulation[] simulations)
        {
            simulationCount = simulations.Length;
            foreach (Simulation simulation in simulations)
            {
                totalAliveCells += simulation.CellCount;
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
                switch (render.CheckKeyPress())
                {
                    case 32:
                        render.IsPaused = timer.Enabled;
                        timer.Enabled = !timer.Enabled;
                        if (!timer.Enabled)
                        {
                            PrintSimulations();
                        }
                        break;
                    case 13:
                        if (!timer.Enabled)
                        {
                            renderList = render.AskRenderList(simulationCount);
                            PrintSimulations();
                        }
                        break;
                    case 27:
                        timer.Stop();
                        render.IsPaused = false;
                        render.PrintSimulationMenu(liveGames, simulationCount, totalAliveCells);
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
            bool[][] oldGrid;
            foreach (var simulation in simulations)
            {
                if (simulation.IsActive)
                {
                    liveGames--;
                    totalAliveCells -= simulation.CellCount;
                    oldGrid = simulation.Grid;
                    
                    simulation.NextIteration();
                    totalAliveCells += simulation.CellCount;

                    if (simulation.CellCount > 0 || !CompareGrids(oldGrid, simulation.Grid))
                    {
                        liveGames++;
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
        /// Compares two grids to see if they are the same
        /// </summary>
        /// <param name="firstGrid">A grid that is to be compared with</param>
        /// <param name="secondGrid">A grid that is to be compared with</param>
        /// <returns>True if they are the same, false if they are not</returns>
        private bool CompareGrids(bool[][] firstGrid, bool[][] secondGrid)
        {
            for (int x = 0; x < firstGrid.Length; x++)
            {
                for (int y = 0; y < firstGrid[0].Length; y++)
                {
                    if (firstGrid[x][y] != secondGrid[x][y])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Calls renderer to print menu and simulations in render list
        /// </summary>
        private void PrintSimulations()
        {
            render.PrintSimulationMenu(liveGames, simulationCount ,totalAliveCells);
            for (int i = 0; i < renderList.Count; i++)
            {
                render.PrintSimulation(simulations[renderList[i]], renderList[i]);
            }
        }
    }
}
