using System.Collections.Generic;

namespace Savanna.Animals
{
    /// <summary>
    /// Animal that is a predator. Special move is to pounce on its prey
    /// </summary>
    public class Lion : Animal
    {
        /// <summary>
        /// Animal that is a predator. Special move is to pounce on its prey
        /// </summary>
        public Lion() : base()
        {
            IsPredator = true;
            VisionRange = 10;
            SpecialActionCooldownReset = 10;
            SpecialActionCooldown = 0;
        }

        /// <summary>
        /// Lion special move: pounce on prey
        /// </summary>
        /// <param name="relativeX">x coordinate of spotted animal</param>
        /// <param name="relativeY">y coordinate of spotted animal</param>
        /// <returns>True if animal did special action</returns>
        public override bool DoSpecialAction(int relativeX, int relativeY)
        {
            if (SpecialActionCooldown == 0)
            {
                X += relativeX;
                Y += relativeY;
                SpecialActionCooldown = SpecialActionCooldownReset;
                return true;
            }
            else
            {
                if (SpecialActionCooldown > 0)
                    SpecialActionCooldown--;
                return false;
            }
        }

        /// <summary>
        /// Animal looks around to determine where to move, also tries to eat any animal around itself
        /// </summary>
        /// <param name="animals">List of animals that current animal can look at</param>
        public override void Look(ref List<Animal> animals)
        {
            // Look around and move in a direction
            base.Look(ref animals);

            // Try to eat a herbivore all around itself
            Eat(ref animals);

        }

        /// <summary>
        /// Method for Lion to eat any herbivores around itself
        /// </summary>
        /// <param name="animals">List of animals for Lion to try to eat</param>
        public void Eat(ref List<Animal> animals)
        {
            for (int i = 0; i < animals.Count; i++)
            {
                if (animals[i] != this && CheckIfAnimalIsNear(animals[i], 1) && !animals[i].IsPredator)
                {
                    // Iteration 2: add health from eating
                    // Health += 10;
                    animals.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// Returns icon for animal to be used in the renderer
        /// </summary>
        /// <returns>Icon of lion for renderer: "L"</returns>
        public override string ReturnIcon()
        {
            return "L";
        }
    }
}
