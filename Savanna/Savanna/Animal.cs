using System;
using System.Collections.Generic;

namespace Savanna
{
    /// <summary>
    /// Abstract class of animal that needs to be implemented by the actual animal classes
    /// </summary>
    public abstract class Animal
    {
        /// <summary>
        /// X coordinates of animal
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Y coordinates of animal
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Range of vision of animal
        /// </summary>
        public int VisionRange { get; private protected set; }

        /// <summary>
        /// Type of animal, used for rendering it on screen
        /// </summary>
        public bool IsPredator { get; private protected set; }

        /// <summary>
        /// Number of turns that animal has to wait to use special action after using it
        /// </summary>
        public int SpecialActionCooldownReset { get; private protected set; }

        /// <summary>
        /// Cooldown for doing special action
        /// </summary>
        public int SpecialActionCooldown { get; private protected set; }

        /// <summary>
        /// Application that owns this animal
        /// </summary>
        private protected Application Owner;

        /// <summary>
        /// Used to generate random actions for animal
        /// </summary>
        private protected Random rand;

        /// <summary>
        /// Abstract class of animal that needs to be implemented by the actual animal classes
        /// </summary>
        public Animal(Application owner)
        {
            rand = new Random();
            Spawn();
            Owner = owner;
        }

        /// <summary>
        /// Spawns animal in random position
        /// </summary>
        public void Spawn()
        {
            X = rand.Next(10);
            Y = rand.Next(10);
        }

        /// <summary>
        /// Animal moves randomly to one of it's surrounding spaces
        /// </summary>
        public void Move()
        {
            X += rand.Next(3) - 1;
            Y += rand.Next(3) - 1;
        }

        /// <summary>
        /// Moves animal in a direction to flee or attack
        /// </summary>
        /// <param name="direction">Direction as a clock with values 0-7, 0 being up</param>
        public void Move(int direction)
        {
            switch (direction)
            {
                case 0:
                    X++;
                    break;
                case 1:
                    X++;
                    Y++;
                    break;
                case 2:
                    Y++;
                    break;
                case 3:
                    Y++;
                    X--;
                    break;
                case 4:
                    X--;
                    break;
                case 5:
                    X--;
                    Y--;
                    break;
                case 6:
                    Y--;
                    break;
                case 7:
                    Y--;
                    X++;
                    break;
            }
        }

        /// <summary>
        /// Animal looks around to determine where to move
        /// </summary>
        public virtual void Look(List<Animal> animals)
        {
            double distanceToAnimal;
            double closestAnimal = VisionRange + 1;
            int closestAnimalX = X;
            int closestAnimalY = Y;
            bool sameAnimalInSight = false;
            bool differentAnimalInSight = false;

            // Look around to see any threats or friendly animals and decide preferred direction
            foreach (Animal animal in animals)
            {
                // ((x1-x2)^2 + (y1-y2)^2) < d^2
                distanceToAnimal = Math.Sqrt(((X - animal.X) * (X - animal.X)) + ((Y - animal.Y) * (Y - animal.Y)));
                
                if (distanceToAnimal < VisionRange)
                {
                    if (differentAnimalInSight)
                    {
                        if (IsPredator != animal.IsPredator && distanceToAnimal < closestAnimal)
                        {
                            closestAnimal = distanceToAnimal;
                            closestAnimalX = animal.X;
                            closestAnimalY = animal.Y;
                        }
                    }
                    else
                    {
                        if (IsPredator != animal.IsPredator)
                        {
                            differentAnimalInSight = true;
                            closestAnimal = distanceToAnimal;
                            closestAnimalX = animal.X;
                            closestAnimalY = animal.Y;
                        }
                        else
                        {
                            if (distanceToAnimal < closestAnimal)
                            {
                                sameAnimalInSight = true;
                                closestAnimal = distanceToAnimal;
                            }
                        }
                    }
                }
            }

            // Movement or special move
            if (differentAnimalInSight)
            {
                if (!DoSpecialAction(closestAnimalX - X, closestAnimalY - Y))
                {
                    Move(DecideDirection(closestAnimalX - X, closestAnimalY - Y, false));
                }
            }
            else if (sameAnimalInSight)
            {
                Move(DecideDirection(closestAnimalX - X, closestAnimalY - Y, true));
            }
            else
            {
                Move();
            }
        }

        /// <summary>
        /// Animal decides in which direction to move in
        /// </summary>
        /// <param name="relativeX">Relative x coordinate of spotted animal</param>
        /// <param name="relativeY">Relative y coordinate of spotted animal</param>
        /// <param name="friendly">True if animal will go toward friendly animal, false if running away</param>
        /// <returns>Direction as a clock with values 0-7, 0 being to the right, counterclockwise</returns>
        public int DecideDirection(int relativeX, int relativeY, bool friendly)
        {
            // Calculate angle of animal and direction from angle
            double angle = Math.Atan2(relativeY, relativeX) * 180 / Math.PI;
            if (angle < 0)
                angle = 360 + angle;

            int direction = (int)Math.Round(angle / 45);
            if (direction == 8)
                direction = 0;

            // Reverse direction if animal is not friendly
            if (!friendly && !IsPredator)
            {
                if (X < 4)
                {
                    direction += 4;
                }
                else
                {
                    direction -= 4;
                }
            }

            return direction;
        }

        /// <summary>
        /// Animal does special action
        /// </summary>
        /// <param name="relativeX">x coordinate of spotted animal</param>
        /// <param name="relativeY">y coordinate of spotted animal</param>
        /// <returns>True if animal did special action</returns>
        public abstract bool DoSpecialAction(int relativeX, int relativeY);
    }
}
