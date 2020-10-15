using System.Text.Json;

namespace GameOfLife
{
    /// <summary>
    /// Handles saving to and loading from files for the game
    /// </summary>
    public class FileHandler : IFileHandler
    {
        private string FileName;
        private readonly IFileSystem fileSystem;

        /// <summary>
        /// Handles saving to and loading from a file for the game
        /// </summary>
        public FileHandler()
        {
            fileSystem = new FileSystem();
        }

        /// <summary>
        /// Handles saving to and loading from a file for the game
        /// </summary>
        /// <param name="fileName">Name of the file to be used</param>
        public FileHandler(string fileName)
        {
            SetFileName(fileName);
            fileSystem = new FileSystem();
        }

        /// <summary>
        /// Handles saving to and loading from a file for the game
        /// </summary>
        /// <param name="fileName">Name of the file to be used</param>
        /// <param name="fileSystem">File system to be used</param>
        public FileHandler(string fileName, IFileSystem fileSystem)
        {
            SetFileName(fileName);
            this.fileSystem = fileSystem;
        }

        /// <summary>
        /// Sets the name of the file that is to be handled
        /// </summary>
        /// <param name="fileName">Name of th file to be use</param>
        public void SetFileName(string fileName)
        {
            FileName = @"./" + fileName + ".txt";
        }

        /// <summary>
        /// Saves information about game to file
        /// </summary>
        /// <param name="simulations">Array of simulations to save</param>
        public void Save(Simulation[] simulations)
        {
            fileSystem.WriteAllText(FileName, JsonSerializer.Serialize(simulations));
        }

        /// <summary>
        /// Loads information about simulation from file and returns it as a string representing the JSON structure
        /// </summary>
        public Simulation[] Load()
        {
            return JsonSerializer.Deserialize<Simulation[]>(fileSystem.ReadAllText(FileName));
        }
    }
}
