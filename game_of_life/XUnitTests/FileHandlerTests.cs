using GameOfLife;
using Xunit;
using Moq;

namespace XUnitTests
{
    public class FileHandlerTests
    {
        private FileHandler testFileHandler;
        private const string jsonString = "[{\"IterationCount\":1,\"CellCount\":0,\"Rows\":10,\"Columns\":10,\"Grid\":[[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false]],\"isActive\":true},{\"IterationCount\":1,\"CellCount\":0,\"Rows\":10,\"Columns\":10,\"Grid\":[[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false]],\"isActive\":true}]";

        public FileHandlerTests()
        {

        }

        [Fact]
        public void TestLoad()
        {
            // Arrange
            var fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonString);
            testFileHandler = new FileHandler("", fileSystem.Object);

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

        [Fact]
        public void TestSave()
        {
            // Arrange
            var fileSystemMock = new Mock<IFileSystem>();
            testFileHandler = new FileHandler("", fileSystemMock.Object);
            Simulation[] simulations = new Simulation[]
            {
                new Simulation()
                {
                    IterationCount = 1,
                    CellCount = 0,
                    Rows = 10,
                    Columns = 10,
                    IsActive = true
                },
                new Simulation()
                {
                    IterationCount = 1,
                    CellCount = 0,
                    Rows = 10,
                    Columns = 10,
                    IsActive = true
                }
            };
            foreach(var simulation in simulations)
            {
                simulation.Initialise();
            }

            // Act
            testFileHandler.Save(simulations);

            // Assert
            fileSystemMock.Verify(x => x.WriteAllText(It.IsAny<string>(), It.IsAny<string>()));
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
            
        }
    }
}
