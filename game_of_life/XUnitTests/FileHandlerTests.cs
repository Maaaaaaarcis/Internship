using GameOfLife;
using Xunit;
using Moq;
using System.Runtime.InteropServices.ComTypes;

namespace XUnitTests
{
    public class FileHandlerTests
    {
        private FileHandler testFileHandler;
        private const string jsonString = "[{\"IterationCount\":1,\"CellCount\":0,\"Rows\":10,\"Columns\":10,\"Grid\":[[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false]],\"isActive\":true},{\"IterationCount\":1,\"CellCount\":0,\"Rows\":10,\"Columns\":10,\"Grid\":[[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false],[false,false,false,false,false,false,false,false,false,false]],\"isActive\":true}]";
        private Mock<IFileSystem> fileSystemMock = new Mock<IFileSystem>();

        public FileHandlerTests()
        {
            testFileHandler = new FileHandler("", fileSystemMock.Object);
        }

        [Fact]
        public void Load_Should_ReturnSimulationArray_When_SerialisedSimulationArrayIsPassed()
        {
            // Arrange
            fileSystemMock.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonString);
            
            // Act
            Simulation[] simulations = testFileHandler.Load();

            // Assert
            Assert.NotNull(simulations);
        }

        [Fact]
        public void Save_Should_PassSerialisedSimulations_When_SimulationArrayIsPassed()
        {
            // Arrange
            Simulation[] simulations = InitialiseTestSimulations();

            // Act
            testFileHandler.Save(simulations);

            // Assert
            fileSystemMock.Verify(x => x.WriteAllText(It.IsAny<string>(), It.IsAny<string>()));
        }

        private Simulation[] InitialiseTestSimulations()
        {
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

            foreach (var simulation in simulations)
            {
                simulation.Initialise();
            }

            return simulations;
        }
    }
}
