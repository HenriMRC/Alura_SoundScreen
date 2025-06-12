using screensound.database.dal;
using screensound.models;
using System;

namespace screensound.menu
{
    internal class RegisterArtistMenu : Menu
    {
        public override string GetOptionInstruction(int optionIndex)
        {
            return $"Type {optionIndex} to register an artist";
        }

        public override void Run(DAL<Artist> artistDal, DAL<Music> musicDal)
        {
            ShowOptionTitle("Artists registry");
            Console.Write("Type the artist's name which you wish to register: ");

            string? name;
            while (true)
            {
                name = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(name))
                    break;

                Console.Write("Artist name cannot be empty. Try again: ");
            }

            Console.Write("Type the artist's biography which you wish to register: ");
            string? bio;
            while (true)
            {
                bio = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(bio))
                    break;

                Console.Write("Artist name cannot be empty. Try again: ");
            }

            Artist artista = new(name, bio);
            artistDal.Add(artista);
            Console.WriteLine($"{name} was successfully registered!");
        }
    }
}
