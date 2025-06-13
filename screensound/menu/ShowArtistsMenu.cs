using screensound.database.dal;
using screensound.models;
using System;

namespace screensound.menu
{
    internal class ShowArtistsMenu : Menu
    {
        public override string GetOptionInstruction(int optionIndex)
        {
            return $"Type {optionIndex} to print the artists";
        }

        public override void Run(DAL<Artist> artistDal, DAL<Music> musicDal)
        {
            ShowOptionTitle("Showing all registered artists on our application");

            foreach (Artist artist in artistDal.GetList())
                Console.WriteLine($"Artist:\n{artist}");
        }
    }
}
