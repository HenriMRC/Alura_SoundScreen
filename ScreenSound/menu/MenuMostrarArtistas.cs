using screensound.database;
using screensound.models;
using System;
using System.Collections.Generic;

namespace screensound.menu
{
    internal class MenuMostrarArtistas : Menu
    {
        public override void Executar(ArtistDAL dal)
        {
            base.Executar(dal);
            ExibirTituloDaOpcao("Showing all registered artists on our application");

            foreach (Artist artist in dal.GetList())
                Console.WriteLine($"Artist:\n{artist}");

            Console.WriteLine("\nType any key to return to the main menu");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
