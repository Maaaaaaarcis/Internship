using Savanna.Animals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Savanna
{
    /// <summary>
    /// Contains logic to run the "Savanna" game
    /// </summary>
    public class Application
    {
        /// <summary>
        /// List of animals in play
        /// </summary>
        public List<Animal> Animals;

        /// <summary>
        /// Timer that handles starting animal actions
        /// </summary>
        public Timer timer;

        /// <summary>
        /// Bool for timer so that antelopes move every 2nd timer tick
        /// </summary>
        private bool HerbivoreMove;

        /// <summary>
        /// Contains logic to run the "Savanna" game
        /// </summary>
        public Application()
        {
            Animals = new List<Animal>();
            HerbivoreMove = false;
            timer = new Timer(500);
            timer.Elapsed += DoTimerEvent;
        }

        /// <summary>
        /// Startup method for the application
        /// </summary>
        public void Start()
        {
            Renderer.RenderWelcomeMessage();
            Loop();
            Renderer.RenderExitMessage();
        }

        /// <summary>
        /// Puts game into loop and asks renderer to listen to user key inputs
        /// </summary>
        private void Loop()
        {
            timer.Enabled = true;
            while (true)
            {
                switch (Renderer.CheckKeyPress())
                {
                    case 27:    // Escape key
                        timer.Stop();
                        return;
                    case 65:    // A key
                        GenerateAnimal('A');
                        break;
                    case 76:    // L key
                        GenerateAnimal('L');
                        break;
                }
            }
        }

        /// <summary>
        /// Does animal actions; Lions move every tick, Antelopes move every 2 ticks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoTimerEvent(object sender, ElapsedEventArgs e)
        {
            HerbivoreMove = !HerbivoreMove;

            foreach(Animal animal in Animals)
            {
                if ((!animal.IsPredator && HerbivoreMove) || animal.IsPredator)
                {
                    animal.Look(Animals.Where(x => x != animal).ToList());
                }
            }

            Renderer.RenderField(Animals);
        }

        /// <summary>
        /// Generates a new animal on the field
        /// </summary>
        /// <param name="animalType">Type of animal to be generated</param>
        private void GenerateAnimal(char animalType)
        {
            Animal animal;

            switch (animalType)
            {
                case 'A':
                    animal = new Antelope(this);
                    break;
                case 'L':
                    animal = new Lion(this);
                    break;
                default:
                    throw new ArgumentException();
            }

            Animals.Add(animal);
        }

        /// <summary>
        /// Method for lions to eat antelope
        /// </summary>
        /// <param name="x">X coordinate of antelope</param>
        /// <param name="y">Y coordinate of antelope</param>
        /// <returns>Number of antelopes eaten on coordinates</returns>
        public int EatAnimal(int x, int y)
        {
            int i = 0;

            foreach (Antelope antelope in Animals)
            {
                if (antelope.X == x && antelope.Y == y)
                {
                    Animals.Remove(antelope);
                    i++;
                }
            }

            return i;
        }
    }
}
