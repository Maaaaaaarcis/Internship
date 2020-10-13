using GameOfLife;
using Xunit;
using Moq;

namespace XUnitTests
{
    public class FileHandlerTests
    {
        private Simulation[] testSimulations;
        private readonly Mock<IFileHandler> fileHandlerMock = new Mock<IFileHandler>();

        public FileHandlerTests()
        {

        }

        [Fact]
        public void TestLoad()
        {
            // Arrange
            bool[][] emptyGrid = new bool[10][];
            for (int i = 0; i < 10; i++)
            {
                emptyGrid[i] = new bool[10];
            }

            fileHandlerMock.Setup(x => x.Load()).Returns(new Simulation[]
            {
                new Simulation
                {
                    IterationCount = 1,
                    CellCount = 0,
                    Rows = 10,
                    Columns = 10,
                    IsActive = true,
                    Grid = emptyGrid
                }
            });

            // Act
            testSimulations = fileHandlerMock.Object.Load();

            // Assert
            Assert.Equal(emptyGrid, testSimulations[0].Grid);
        }
    }
}
