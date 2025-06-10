using screensound.database.dal;
using screensound.models;
using System;

namespace screensound.menu
{
    internal class ShowMusicsMenu : Menu
    {
        public override void Executar(DAL<Artist> dal)
        {
            base.Executar(dal);
            ExibirTituloDaOpcao("Show artist details");
            Console.Write("Type the artist's name which you wish to know better: ");

            string? name;
            while (true)
            {
                name = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(name))
                    break;

                Console.Write("Artist name cannot be empty. Try again: ");
            }

            Artist? artist = dal.First(a => a.Name.Equals(name));
            if (artist == null)
            {
                Console.Write("Artist name not found!");
            }
            else
            {
                Console.WriteLine("\nDiscography:");
                artist.ShowDiscography();
            }

            Console.WriteLine("\nPress any key to return to the main menu");
            Console.ReadKey();
            Console.Clear();
        }
    }
}