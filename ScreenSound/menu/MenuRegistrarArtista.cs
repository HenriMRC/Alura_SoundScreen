using screensound.database;
using screensound.models;
using System;
using System.Threading;

namespace screensound.menu
{
    internal class MenuRegistrarArtista : Menu
    {
        public override void Executar(ArtistDAL dal)
        {
            base.Executar(dal);
            ExibirTituloDaOpcao("Artists registry");
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
            dal.Add(artista);
            Console.WriteLine($"{name} was successfully registered!");
            Thread.Sleep(4000);
            Console.Clear();
        }
    }
}
