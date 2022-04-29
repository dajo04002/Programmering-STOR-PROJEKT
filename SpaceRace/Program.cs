namespace SpaceRace2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (SpaceRace.Game1 game = new SpaceRace.Game1())
            {
                game.Run();
            }
        }
    }
}
