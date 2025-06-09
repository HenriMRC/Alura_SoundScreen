using screensound.database;
using screensound.models;
using System;
using System.Threading;

namespace screensound.menu
{
    internal class MenuRegistrarMusica : Menu
    {
        public override void Executar(ArtistDAL dal)
        {
            base.Executar(dal);

            ExibirTituloDaOpcao("Musics registry");
            Console.Write("Type the artist's name whose music you wish to register: ");

            string? name;
            while (true)
            {
                name = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(name))
                    break;

                Console.Write("Artist name cannot be empty. Try again: ");
            }

            Artist? artist = dal.GetFirstByName(name);
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

                    Console.Write("Artist name cannot be empty. Try again: ");
                }

                artist.AddMusic(new Musica(music));
                Console.WriteLine($"The music {music} from {name} was successfully registered!");
                Thread.Sleep(4000);
                Console.Clear();
            }

            Console.WriteLine("\nPress any key to return to the main menu");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
