using Microsoft.EntityFrameworkCore.ChangeTracking;
using screensound.database;
using screensound.menu;
using screensound.models;
using System;
using System.Collections.Generic;

namespace screensound
{
    public class Program
    {
        public static void Main()
        {
            try
            {
                ScreenSoundContext context = new();
                ArtistDAL artistDAL = new(context);
                Artist newArtist = new("Foo Fighters", "Foo Fighters é uma banda de rock alternativo americana formada por Dave Grohl em 1995.");

                EntityEntry<Artist> result = artistDAL.Add(newArtist);

                foreach (Artist artist in artistDAL.GetList())
                    Console.WriteLine($"{artist}\n");

                result.Entity.Bio = string.Empty;
                result = artistDAL.Update(result.Entity);
                Console.WriteLine("\n\n   ========== UPDATED ==========   \n\n");

                foreach (Artist artist in artistDAL.GetList())
                    Console.WriteLine($"{artist}\n");


                result = artistDAL.Remove(result.Entity);
                Console.WriteLine("\n\n   ========== DELETED ==========   \n\n");
                Console.WriteLine($"{result.Entity}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return;

            Artist ira = new("Ira!", "Banda Ira!");
            Artist beatles = new("The Beatles", "Banda The Beatles");

            Dictionary<string, Artist> artistasRegistrados = new()
            {
                { ira.Name, ira },
                { beatles.Name, beatles }
            };

            Dictionary<int, Menu> opcoes = new()
            {
                { 1, new MenuRegistrarArtista() },
                { 2, new MenuRegistrarMusica() },
                { 3, new MenuMostrarArtistas() },
                { 4, new MenuMostrarMusicas() },
                { -1, new MenuSair() }
            };

            void ExibirLogo()
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

            void ExibirOpcoesDoMenu()
            {
                ExibirLogo();
                Console.WriteLine("\nDigite 1 para registrar um artista");
                Console.WriteLine("Digite 2 para registrar a música de um artista");
                Console.WriteLine("Digite 3 para mostrar todos os artistas");
                Console.WriteLine("Digite 4 para exibir todas as músicas de um artista");
                Console.WriteLine("Digite -1 para sair");

                Console.Write("\nDigite a sua opção: ");
                string opcaoEscolhida = Console.ReadLine();
                int opcaoEscolhidaNumerica = int.Parse(opcaoEscolhida);

                if (opcoes.ContainsKey(opcaoEscolhidaNumerica))
                {
                    Menu menuASerExibido = opcoes[opcaoEscolhidaNumerica];
                    menuASerExibido.Executar(artistasRegistrados);
                    if (opcaoEscolhidaNumerica > 0) ExibirOpcoesDoMenu();
                }
                else
                {
                    Console.WriteLine("Opção inválida");
                }
            }

            ExibirOpcoesDoMenu();
        }
    }
}