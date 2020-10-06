using System.Text.Json;
using System.Text.Json.Serialization;

namespace GameOfLife
{
    /// <summary>
    /// Handles saving to and loading from files for the game
    /// </summary>
    public class FileHandler
    {
        private string FileName;

        /// <summary>
        /// Handles saving to and loading from a file for the game
        /// </summary>
        /// <param name="fileName">Name of the file to be used</param>
        public FileHandler(string fileName)
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
