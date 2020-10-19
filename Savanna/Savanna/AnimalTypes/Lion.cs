namespace Savanna.Animals
{
    public class Lion : Animal
    {
        public Lion(Application owner) : base(owner)
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

        public void Eat(int x, int y)
        {
            // Iteration 2: add health from int EatAnimal()
            Owner.EatAnimal(x, y);
        }
    }
}
