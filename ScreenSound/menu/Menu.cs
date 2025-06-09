using screensound.database;
using System;

namespace screensound.menu
{
    internal class Menu
    {
        public static void ExibirTituloDaOpcao(string titulo)
        {
            int quantidadeDeLetras = titulo.Length;
            string asteriscos = string.Empty.PadLeft(quantidadeDeLetras, '*');
            Console.WriteLine(asteriscos);
            Console.WriteLine(titulo);
            Console.WriteLine(asteriscos + "\n");
        }

        public virtual void Executar(ArtistDAL dal)
        {
            Console.Clear();
        }
    }
}
