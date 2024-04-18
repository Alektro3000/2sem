namespace HanoTower
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            int n = 15;

            HanoSolution Sol = new HanoSolution();
            var Log = Sol.Solve(n);
            var Game = new App2D(Log,n);
            Game.Run();

        }
    }
}