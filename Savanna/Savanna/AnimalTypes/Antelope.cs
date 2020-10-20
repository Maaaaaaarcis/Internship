using System;

namespace Savanna
{
    public class Antelope : Animal
    {
        public Antelope(Application owner) : base(owner)
        {
            IsPredator = false;
            VisionRange = 5;
            SpecialActionCooldownReset = 15;
            SpecialActionCooldown = 0;
        }

        /// <summary>
        /// Antilope special action: run 3 tiles in one turn in a single direction
        /// </summary>
        /// <param name="relativeX">x coordinate of spotted animal</param>
        /// <param name="relativeY">y coordinate of spotted animal</param>
        /// <returns>True if animal did special action</returns>
        public override bool DoSpecialAction(int relativeX, int relativeY)
        {
            if (SpecialActionCooldown == 0)
            {
                int direction = DecideDirection(relativeX, relativeY, false);
                for (int i = 0; i < 3; i++)
                {
                    Move(direction);
                }
                return true;
            }
            else
            {
                if (SpecialActionCooldown > 0)
                    SpecialActionCooldown--;
                return false;
            }
        }
    }
}
