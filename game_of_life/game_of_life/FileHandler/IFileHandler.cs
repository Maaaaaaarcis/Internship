using System;
using System.Collections.Generic;
using System.Text;

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
}
