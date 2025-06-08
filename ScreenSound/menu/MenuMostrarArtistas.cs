using screensound.models;
using System;
using System.Collections.Generic;

namespace screensound.menu
{
    internal class MenuMostrarArtistas : Menu
    {
        public override void Executar(Dictionary<string, Artist> musicasRegistradas)
        {
            base.Executar(musicasRegistradas);
            ExibirTituloDaOpcao("Exibindo todos os artistas registradas na nossa aplicação");

            foreach (string artista in musicasRegistradas.Keys)
            {
                Console.WriteLine($"Artista: {artista}");
            }

            Console.WriteLine("\nDigite uma tecla para voltar ao menu principal");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
