using System.Text.Json;

namespace GameOfLife
{
    /// <summary>
    /// File handler interface to be mocked in testing
    /// </summary>
    public interface IFileHandler
    {
        /// <summary>
        /// Sets the name of the file that is to be handled
        /// </summary>
        /// <param name="fileName">Name of th file to be use</param>
        void SetFileName(string fileName);

        /// <summary>
        /// Saves information about game to file
        /// </summary>
        /// <param name="simulations">Array of simulations to save</param>
        void Save(Simulation[] simulations);

        /// <summary>
        /// Loads information about simulation from file and returns it as a string representing the JSON structure
        /// </summary>
        Simulation[] Load();
    }

    /// <summary>
    /// Handles saving to and loading from files for the game
    /// </summary>
    public class FileHandler : IFileHandler
    {
        private string FileName;

        /// <summary>
        /// Handles saving to and loading from a file for the game
        /// </summary>
        public FileHandler()
        {

        }

        /// <summary>
        /// Handles saving to and loading from a file for the game
        /// </summary>
        /// <param name="fileName">Name of the file to be used</param>
        public FileHandler(string fileName)
        {
            SetFileName(fileName);
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
            System.IO.File.WriteAllText(FileName, JsonSerializer.Serialize(simulations));
        }

        /// <summary>
        /// Loads information about simulation from file and returns it as a string representing the JSON structure
        /// </summary>
        public Simulation[] Load()
        {
            return JsonSerializer.Deserialize<Simulation[]>(System.IO.File.ReadAllText(FileName));
        }
    }
}
