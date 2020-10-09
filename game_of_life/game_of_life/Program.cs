namespace GameOfLife
{
    /// <summary>
    /// Startup and restart class for Game of Life
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Startup method for the Game of Life
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            bool restart;
            do
            {
                Game game = new Game();
                restart = game.Play();
            } while (restart);
        }
    }
}
