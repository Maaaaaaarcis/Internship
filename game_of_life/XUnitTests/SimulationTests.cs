using GameOfLife;
using Xunit;

namespace XUnitTests
{
    public class SimulationTests
    {
        private readonly Simulation testingSimulation;

        public SimulationTests()
        {
            testingSimulation = new Simulation
            {
                IterationCount = 1,
                CellCount = 0,
                Rows = 10,
                Columns = 10,
                IsActive = true
            };
            testingSimulation.Initialise();
        }

        /*[Theory]
        [InlineData(1, new int[,] {
            {1, 1 },
            {1, 2 },
            {2, 1 },
            {2, 2 }
        })]
        public void TestSimulationNextIteration(int period, int[,] aliveCellCoords)
        {
            bool[][] testGrid = testingSimulation.Grid;

            

            for (int i = 0; i < 10; i++)
            {
                for (int x = 0; x < period; x++)
                {
                    testingSimulation.NextIteration();
                }
                Assert.Equal(testGrid, testingSimulation.Grid);
            }
        }*/
    }
}
