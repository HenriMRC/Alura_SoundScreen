using screensound.core.data.dal;
using screensound.core.models;
using System;

namespace screensound.menu
{
    internal abstract class Menu
    {
        public abstract string GetOptionInstruction(int optionIndex);

        public static void ShowOptionTitle(string titulo)
        {
            int quantidadeDeLetras = titulo.Length;
            string asteriscos = string.Empty.PadLeft(quantidadeDeLetras, '*');
            Console.WriteLine(asteriscos);
            Console.WriteLine(titulo);
            Console.WriteLine(asteriscos + "\n");
        }

        public virtual void PreRun()
        {
            Console.Clear();
        }

        public abstract void Run(DAL<Artist> artistDal, DAL<Music> musicDal);

        public virtual void PostRun()
        {
            Console.WriteLine("\nPress any key to return to the Main Menu.");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
