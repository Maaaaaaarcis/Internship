using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GameOfLife
{
    /// <summary>
    /// Handles saving to and loading from files for the game
    /// </summary>
    public class FileHandler
    {
        private string FileName { get; set; }

        /// <summary>
        /// Handles saving to and loading from a file for the game
        /// </summary>
        /// <param name="fileName">Name of the file to be used</param>
        public FileHandler(string fileName)
        {
            FileName = @"./" + fileName + ".txt";
        }

        /// <summary>
        /// Saves information about simulation to file
        /// </summary>
        public void Save(string json)
        {
            System.IO.File.WriteAllText(FileName, json);
        }

        /// <summary>
        /// Loads information about simulation from file and returns it as a JSON document
        /// </summary>
        public string Load()
        {
            return System.IO.File.ReadAllText(FileName);
        }
    }
}
