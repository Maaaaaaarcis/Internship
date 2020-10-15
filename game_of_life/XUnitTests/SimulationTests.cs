using GameOfLife;
using Xunit;

namespace XUnitTests
{
    public class SimulationTests
    {
        private Simulation testingSimulation;

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

        [Theory]
        [InlineData(1,
            new int[] { 1, 1 },
            new int[] { 1, 2 },
            new int[] { 2, 1 },
            new int[] { 2, 2 }
            )] // Block
        [InlineData(2,
            new int[] { 1, 2 },
            new int[] { 2, 2 },
            new int[] { 3, 2 }
            )] // Blinker
        [InlineData(2,
            new int[] { 2, 2 },
            new int[] { 2, 3 },
            new int[] { 2, 4 },
            new int[] { 3, 1 },
            new int[] { 3, 2 },
            new int[] { 3, 3 }
            )] // Toad
        public void NextIteration_Should_RepeatItself_When_RepeatingFormationIsGiven(int period, params int[][] aliveCellCoords)
        {
            // Arrange
            bool[][] initialGrid = InitialiseTestGrid(aliveCellCoords);

            // Act
            for (int x = 0; x < period; x++)
            {
                testingSimulation.NextIteration();
            }

            // Assert
            Assert.Equal(initialGrid, testingSimulation.Grid);
        }

        private bool[][] InitialiseTestGrid(params int[][] aliveCellCoords)
        {
            bool[][] testGrid = testingSimulation.Grid;

            for (int i = 0; i < aliveCellCoords.Length; i++)
            {
                testGrid[aliveCellCoords[i][0]][aliveCellCoords[i][1]] = true;
            }

            testingSimulation.Grid = testGrid;

            return testGrid;
        }
    }
}
