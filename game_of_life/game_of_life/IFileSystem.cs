using System;
using System.Collections.Generic;
using System.Text;

namespace GameOfLife
{
    /// <summary>
    /// Handles writting to and reading from file
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// Reads all text from a file
        /// </summary>
        /// <param name="filePath">Path of the file to be read</param>
        /// <returns>All text in file</returns>
        string ReadAllText(string filePath);

        /// <summary>
        /// Writes text to a file
        /// </summary>
        /// <param name="filePath">Path of the file to be written to</param>
        /// <param name="textToWrite">Text that will be written to file</param>
        void WriteAllText(string filePath, string textToWrite);
    }
}
