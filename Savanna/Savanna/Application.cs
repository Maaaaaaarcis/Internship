using Savanna.Animals;
using System.Collections.Generic;
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
        private List<Animal> AnimalsInPlay;

        /// <summary>
        /// Timer that handles starting animal actions
        /// </summary>
        private Timer timer;

        /// <summary>
        /// Bool for timer so that herbivores move every 2nd timer tick
        /// </summary>
        private bool HerbivoreMove;

        /// <summary>
        /// Renderer that handles rendering to screen
        /// </summary>
        private Renderer renderer;

        /// <summary>
        /// Contains logic to run the "Savanna" game
        /// </summary>
        public Application()
        {
            AnimalsInPlay = new List<Animal>();
            renderer = new Renderer();
            HerbivoreMove = false;
            timer = new Timer(500);
            timer.Elapsed += DoTimerEvent;
        }

        /// <summary>
        /// Startup method for the application
        /// </summary>
        public void Start()
        {
            renderer.RenderWelcomeMessage();
            Loop();
            renderer.RenderExitMessage();
        }

        /// <summary>
        /// Puts game into loop and asks renderer to listen to user key inputs
        /// </summary>
        private void Loop()
        {
            timer.Enabled = true;
            while (true)
            {
                switch (renderer.CheckKeyPress())
                {
                    case 27:    // Escape key - exit loop
                        timer.Stop();
                        return;
                    case 65:    // A key - add antelope to field
                        //GenerateAnimal('A');
                        AnimalsInPlay.Add(new Antelope());
                        break;
                    case 76:    // L key - add lion to field
                        //GenerateAnimal('L');
                        AnimalsInPlay.Add(new Lion());
                        break;
                    case 32:    // Spacebar - pause
                        timer.Enabled = !timer.Enabled;
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

            for (int i = 0; i < AnimalsInPlay.Count; i++)
            {
                if ((!AnimalsInPlay[i].IsPredator && HerbivoreMove) || AnimalsInPlay[i].IsPredator)
                {
                    AnimalsInPlay[i].Look(ref AnimalsInPlay);
                }
            }

            if (AnimalsInPlay.Count != 0)
            {
                renderer.RenderField(AnimalsInPlay, 0);
            }
        }

        /*/// <summary>
        /// Generates a new animal on the field
        /// </summary>
        /// <param name="animalType">Type of animal to be generated</param>
        private void GenerateAnimal()
        {
            Animal animal = new Animal();

            AnimalsInPlay.Add(animal);
        }*/
    }
}
