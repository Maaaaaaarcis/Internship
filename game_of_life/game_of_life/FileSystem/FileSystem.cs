using System;
using System.Collections.Generic;
using System.Text;

namespace GameOfLife
{
    /// <summary>
    /// Handles writting to and reading from file
    /// </summary>
    public class FileSystem : IFileSystem
    {
        /// <summary>
        /// Handles writting to and reading from file
        /// </summary>
        public FileSystem()
        {

        }

        /// <summary>
        /// Reads all text from a file
        /// </summary>
        /// <param name="filePath">Path of the file to be read</param>
        /// <returns>All text in file</returns>
        public string ReadAllText(string filePath)
        {
            return System.IO.File.ReadAllText(filePath);
        }

        /// <summary>
        /// Writes text to a file
        /// </summary>
        /// <param name="filePath">Path of the file to be written to</param>
        /// <param name="textToWrite">Text that will be written to file</param>
        public void WriteAllText(string filePath, string textToWrite)
        {
            System.IO.File.WriteAllText(filePath, textToWrite);
        }
    }
}
