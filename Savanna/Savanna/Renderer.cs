using System;
using System.Collections.Generic;
using System.Text;

namespace Savanna
{
    /// <summary>
    /// Handles rendering to screen
    /// </summary>
    public static class Renderer
    {
        /// <summary>
        /// Renders welcome message
        /// </summary>
        public static void RenderWelcomeMessage()
        {
            Console.WriteLine("Welcome to the \"Savanna\"!\nPress any button to start!");
            Console.ReadKey(false);
            Console.CursorVisible = false;
        }

        /// <summary>
        /// Renders exit message
        /// </summary>
        public static void RenderExitMessage()
        {
            Console.WriteLine("Press any button to exit program!");
            Console.ReadKey();
        }

        /// <summary>
        /// Checks to see what, if any, button is pressed by the user
        /// </summary>
        /// <returns>Integer representation of pressed key, or -1 if no key is pressed</returns>
        public static int CheckKeyPress()
        {
            if (Console.KeyAvailable)
            {
                return (int)Console.ReadKey(false).Key;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Renders field with animals
        /// </summary>
        /// <param name="animals">List of animals in play to be rendered</param>
        public static void RenderField(List<Animal> animals)
        {
            StringBuilder output = new StringBuilder();

            foreach (Animal animal in animals)
            {
                output.Append(animal.Type + " => X: " + animal.X + ", Y: " + animal.Y + "\n");
            }

            Console.Clear();
            Console.Write(output);
        }
        //public static void RenderField(List<Animal> animals)
        //{
        //    char[,] field = new char[100, 100];

        //    foreach(Animal animal in animals)
        //    {
        //        field[animal.X, animal.Y] = animal.Type;
        //    }

        //    StringBuilder output = new StringBuilder();
        //    char compare = new char();

        //    for (int x = 0; x < 100; x++)
        //    {
        //        for (int y = 0; y < 100; y++)
        //        {
        //            if (field[x, y] != compare)
        //                output.Append(field[x, y]);
        //            else
        //                output.Append(' ');
        //        }
        //        output.Append("\n");
        //    }

        //    Console.Clear();
        //    Console.Write(output);
        //}
    }
}
