namespace Savanna
{
    /// <summary>
    /// Animal that is not a predator. Special move is to dash 
    /// </summary>
    public class Antelope : Animal
    {
        /// <summary>
        /// Animal that is not a predator. Special move is to dash 
        /// </summary>
        public Antelope() : base()
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
                {
                    SpecialActionCooldown--;
                }
                return false;
            }
        }

        /// <summary>
        /// Returns icon for animal to be used in the renderer
        /// </summary>
        /// <returns>Icon of antelope for renderer: "A"</returns>
        public override string ReturnIcon()
        {
            return "A";
        }
    }
}
