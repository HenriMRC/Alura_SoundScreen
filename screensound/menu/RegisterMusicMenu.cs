using screensound.database.dal;
using screensound.core.models;
using System;

namespace screensound.menu
{
    internal class RegisterMusicMenu : Menu
    {
        public override string GetOptionInstruction(int optionIndex)
        {
            return $"Type {optionIndex} to register a music";
        }

        public override void Run(DAL<Artist> artistDal, DAL<Music> musicDal)
        {
            ShowOptionTitle("Musics registry");
            Console.Write("Type the artist's name whose music you wish to register: ");

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
                Console.Write("Type the music's title: ");

                string? music;
                while (true)
                {
                    music = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(music))
                        break;

                    Console.Write("Music title cannot be empty. Try again: ");
                }

                Console.Write("Type the music's year of release: ");

                int yor;
                while (true)
                {
                    string? yorStr = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(yorStr) && int.TryParse(yorStr, out yor))
                        break;

                    Console.Write("Invalid music release year. Try again: ");
                }

                artist.AddMusic(new Music(music) { YearOfRelease = yor });
                Console.WriteLine($"The music {music} from {name} was successfully registered!");
                artistDal.Update(artist);
            }
        }
    }
}
