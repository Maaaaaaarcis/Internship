using GameOfLife;
using Xunit;
using Moq;

namespace XUnitTests
{
    public class FileHandlerTests
    {
        private FileHandler testFileHandler;

        public FileHandlerTests()
        {

        }

        [Fact]
        public void TestLoad()
        {
            // Arrange
            var fileSystem = new TestableFileSystem();
            testFileHandler = new FileHandler("", fileSystem);

            bool[][] emptyGrid = new bool[10][];
            for (int i = 0; i < 10; i++)
                emptyGrid[i] = new bool[10];

            // Act
            Simulation[] simulations = testFileHandler.Load();

            // Assert
            Assert.Equal(2, simulations.Length);
            foreach (var simulation in simulations)
            {
                Assert.Equal(emptyGrid, simulation.Grid);
            }
        }
    }

    class TestableFileSystem : IFileSystem
    {
        public string ReadAllText(string filePath)
        {
            return "[{\"IterationCount\":1,\"CellCount\":0,\"Rows\":10,\"Columns\":10,\"Grid\":[[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false]],\"isActive\":true},{\"IterationCount\":1,\"CellCount\":0,\"Rows\":10,\"Columns\":10,\"Grid\":[[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false]],\"isActive\":true}]";
        }

        public void WriteAllText(string filePath, string textToWrite)
        {
            throw new System.NotImplementedException();
        }
    }
}
