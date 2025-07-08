using screensound.core.data.dal;
using screensound.core.models;
using System;

namespace screensound.menu
{
    internal class ShowMusicsMenu : Menu
    {
        public override string GetOptionInstruction(int optionIndex)
        {
            return $"Type {optionIndex} to print the musics of an artist";
        }

        public override void Run(DAL<Artist> artistDal, DAL<Music> musicDal)
        {
            ShowOptionTitle("Show artist details");
            Console.Write("Type the artist's name which you wish to know better: ");

            string? name;
            while (true)
            {
                name = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(name))
                    break;

                Console.Write("Artist name cannot be empty. Try again: ");
            }

            Artist? artist = artistDal.First(a => a.Name.Equals(name));
            if (artist == null)
            {
                Console.Write("Artist name not found!");
            }
            else
            {
                Console.WriteLine("\nDiscography:");
                artist.ShowDiscography();
            }
        }
    }
}