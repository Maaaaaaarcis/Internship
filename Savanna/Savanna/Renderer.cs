using System;
using System.Collections.Generic;
using System.Text;

namespace Savanna
{
    /// <summary>
    /// Handles rendering to screen
    /// </summary>
    public class Renderer
    {
        /// <summary>
        /// Renders welcome message
        /// </summary>
        public void RenderWelcomeMessage()
        {
            Console.WriteLine("Welcome to the \"Savanna\"!\nPress any button to start!");
            Console.ReadKey(false);
            Console.Clear();
            Console.CursorVisible = false;
        }

        /// <summary>
        /// Renders exit message
        /// </summary>
        public void RenderExitMessage()
        {
            Console.WriteLine("Press any button to exit program!");
            Console.ReadKey();
        }

        /// <summary>
        /// Checks to see what, if any, button is pressed by the user
        /// </summary>
        /// <returns>Integer representation of pressed key, or -1 if no key is pressed</returns>
        public int CheckKeyPress()
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
        /// <param name="index">Index of animal that camera is following</param>
        public void RenderField(List<Animal> animals, int index)
        {
            StringBuilder output = new StringBuilder();

            int visionRange = animals[index].VisionRange;
            int animalX = animals[index].X;
            int animalY = animals[index].Y;
            bool noAnimalSpotted;

            for (int y = -visionRange; y <= visionRange; y++)
            {
                for (int x = -visionRange; x <= visionRange; x++)
                {
                    noAnimalSpotted = true;
                    foreach (Animal animal in animals)
                    {
                        if (animal.X == animalX + x && animal.Y == animalY + y)
                        {
                            output.Append(animal.ReturnIcon());
                            noAnimalSpotted = false;
                            break;
                        }
                    }
                    if (noAnimalSpotted)
                    {
                        output.Append(" ");
                    }
                }
                if (y == 0)
                {
                    output.Append("  X: " + animals[index].X + ", Y: " + animals[index].Y);
                }
                output.Append("\n");
            }

            Console.Clear();
            Console.Write(output);
        }
    }
}
