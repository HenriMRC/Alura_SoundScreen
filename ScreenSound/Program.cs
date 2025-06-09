using screensound.database;
using screensound.menu;
using System;
using System.Collections.Generic;

namespace screensound
{
    public class Program
    {
        public static void Main()
        {
            ExibirOpcoesDoMenu();
        }

        private static void ExibirLogo()
        {
            Console.WriteLine(@"

                       ░██████╗░█████╗░██████╗░███████╗███████╗███╗░░██╗  ░██████╗░█████╗░██╗░░░██╗███╗░░██╗██████╗░
                       ██╔════╝██╔══██╗██╔══██╗██╔════╝██╔════╝████╗░██║  ██╔════╝██╔══██╗██║░░░██║████╗░██║██╔══██╗
                       ╚█████╗░██║░░╚═╝██████╔╝█████╗░░█████╗░░██╔██╗██║  ╚█████╗░██║░░██║██║░░░██║██╔██╗██║██║░░██║
                       ░╚═══██╗██║░░██╗██╔══██╗██╔══╝░░██╔══╝░░██║╚████║  ░╚═══██╗██║░░██║██║░░░██║██║╚████║██║░░██║
                       ██████╔╝╚█████╔╝██║░░██║███████╗███████╗██║░╚███║  ██████╔╝╚█████╔╝╚██████╔╝██║░╚███║██████╔╝
                       ╚═════╝░░╚════╝░╚═╝░░╚═╝╚══════╝╚══════╝╚═╝░░╚══╝  ╚═════╝░░╚════╝░░╚═════╝░╚═╝░░╚══╝╚═════╝░
                       ");
            Console.WriteLine("Boas vindas ao Screen Sound 3.0!");
        }

        private static void ExibirOpcoesDoMenu()
        {
            while (true)
            {
                ScreenSoundContext context = new();
                ArtistDAL dal = new(context);

                Dictionary<int, Menu> opcoes = new()
                {
                    { 1, new MenuRegistrarArtista() },
                    { 2, new MenuRegistrarMusica() },
                    { 3, new MenuMostrarArtistas() },
                    { 4, new MenuMostrarMusicas() },
                    { -1, new MenuSair() }
                };

                ExibirLogo();
                Console.WriteLine("\nDigite 1 para registrar um artista");
                Console.WriteLine("Digite 2 para registrar a música de um artista");
                Console.WriteLine("Digite 3 para mostrar todos os artistas");
                Console.WriteLine("Digite 4 para exibir todas as músicas de um artista");
                Console.WriteLine("Digite -1 para sair");

                Console.Write("\nDigite a sua opção: ");

                int number;
                Menu? menu;
                while (true)
                {
                    string? numberString = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(numberString) &&
                        int.TryParse(numberString, out number) &&
                        opcoes.TryGetValue(number, out menu))
                        break;

                    Console.Write("Option is not valid. Try again: ");
                }

                menu.Executar(dal);

                if (number < 0)
                    break;
            }
        }
    }
}