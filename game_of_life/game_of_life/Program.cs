namespace GameOfLife
{
    class Program
    {
        static void Main(string[] args)
        {
            bool restart;
            do
            {
                Game game = new Game();
                restart = game.Start();
            } while (restart);
        }
    }
}
