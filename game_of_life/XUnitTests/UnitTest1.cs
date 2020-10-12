using GameOfLife;
using Moq;
using Xunit;

namespace XUnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void TestNextIterationBlock()
        {
            Simulation simulation = new Simulation
            {
                IterationCount = 1,
                CellCount = 0,
                Rows = 4,
                Columns = 4,
                IsActive = true
            };
            simulation.Initialise();
            simulation.Grid[1][1] = true;
            simulation.Grid[1][2] = true;
            simulation.Grid[2][1] = true;
            simulation.Grid[2][2] = true;

            bool[][] assertion = simulation.Grid;

            for (int i = 0; i < 5; i++)
            {
                simulation.NextIteration();
                Assert.Equal(assertion, simulation.Grid);
            }
        }

        [Fact]
        public void TestNextIterationBlinker()
        {
            Simulation simulation = new Simulation
            {
                IterationCount = 1,
                CellCount = 0,
                Rows = 5,
                Columns = 5,
                IsActive = true
            };
            simulation.Initialise();
            simulation.Grid[1][2] = true;
            simulation.Grid[2][2] = true;
            simulation.Grid[3][2] = true;

            bool[][] assertion1 = simulation.Grid;
            simulation.NextIteration();
            bool[][] assertion2 = simulation.Grid;

            for (int i = 0; i < 5; i++)
            {
                simulation.NextIteration();
                Assert.Equal(assertion1, simulation.Grid);
                simulation.NextIteration();
                Assert.Equal(assertion2, simulation.Grid);
            }
        }

        [Fact]
        public void TestNextIterationToad()
        {
            Simulation simulation = new Simulation
            {
                IterationCount = 1,
                CellCount = 0,
                Rows = 6,
                Columns = 6,
                IsActive = true
            };
            simulation.Initialise();
            simulation.Grid[2][2] = true;
            simulation.Grid[2][3] = true;
            simulation.Grid[2][4] = true;
            simulation.Grid[3][1] = true;
            simulation.Grid[3][2] = true;
            simulation.Grid[3][3] = true;

            bool[][] assertion1 = simulation.Grid;
            simulation.NextIteration();
            bool[][] assertion2 = simulation.Grid;

            for (int i = 0; i < 5; i++)
            {
                simulation.NextIteration();
                Assert.Equal(assertion1, simulation.Grid);
                simulation.NextIteration();
                Assert.Equal(assertion2, simulation.Grid);
            }
        }

        [Fact]
        public void TestFileHandlingLoad()
        {
            Simulation[] simulations = new Simulation[1];
            simulations[0] = new Simulation
            {
                IterationCount = 1,
                CellCount = 0,
                Rows = 4,
                Columns = 4,
                IsActive = true
            };
            simulations[0].Initialise();
            simulations[0].Grid[1][1] = true;
            simulations[0].Grid[1][2] = true;
            simulations[0].Grid[2][1] = true;
            simulations[0].Grid[2][2] = true;

            var moqFileHandler = new Mock<FileHandler>();
            moqFileHandler.Setup(p => p.Load()).Returns(simulations);
            

        }
    }
}
