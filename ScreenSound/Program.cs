using screensound.database;
using screensound.database.dal;
using screensound.menu;
using screensound.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace screensound
{
    public class Program
    {
        public static void Main()
        {
            ShowMenuOptions();
        }

        private static void ShowLogo()
        {
            Console.WriteLine(@"

                       ░██████╗░█████╗░██████╗░███████╗███████╗███╗░░██╗  ░██████╗░█████╗░██╗░░░██╗███╗░░██╗██████╗░
                       ██╔════╝██╔══██╗██╔══██╗██╔════╝██╔════╝████╗░██║  ██╔════╝██╔══██╗██║░░░██║████╗░██║██╔══██╗
                       ╚█████╗░██║░░╚═╝██████╔╝█████╗░░█████╗░░██╔██╗██║  ╚█████╗░██║░░██║██║░░░██║██╔██╗██║██║░░██║
                       ░╚═══██╗██║░░██╗██╔══██╗██╔══╝░░██╔══╝░░██║╚████║  ░╚═══██╗██║░░██║██║░░░██║██║╚████║██║░░██║
                       ██████╔╝╚█████╔╝██║░░██║███████╗███████╗██║░╚███║  ██████╔╝╚█████╔╝╚██████╔╝██║░╚███║██████╔╝
                       ╚═════╝░░╚════╝░╚═╝░░╚═╝╚══════╝╚══════╝╚═╝░░╚══╝  ╚═════╝░░╚════╝░░╚═════╝░╚═╝░░╚══╝╚═════╝░
                       ");
            Console.WriteLine($"Welcome to Screen Sound {Assembly.GetExecutingAssembly().GetName().Version}!");
        }

        private static void ShowMenuOptions()
        {
            using ScreenSoundContext context = new();
            DAL<Artist> artistDal = new(context);
            DAL<Music> musicDal = new(context);

            Dictionary<int, Menu> options = new()
            {
                { 1, new RegisterArtistMenu() },
                { 2, new RegisterMusicMenu() },
                { 3, new ShowArtistsMenu() },
                { 4, new ShowMusicsMenu() },
                { 5, new ListMusicsOfYearMenu() },
                { -1, new ExitMenu() }
            };

            while (true)
            {
                ShowLogo();
                Console.WriteLine();

                IOrderedEnumerable<KeyValuePair<int, Menu>> orderedOptions =
                    options.OrderBy(p => p.Key < 0 ? int.MaxValue : p.Key);
                foreach (KeyValuePair<int, Menu> option in orderedOptions)
                    Console.WriteLine(option.Value.GetOptionInstruction(option.Key));

                Console.Write("\nChoose one option: ");

                int number;
                Menu? menu;
                while (true)
                {
                    string? numberString = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(numberString) &&
                        int.TryParse(numberString, out number) &&
                        options.TryGetValue(number, out menu))
                        break;

                    Console.Write("Option is not valid. Try again: ");
                }

                menu.PreRun();
                menu.Run(artistDal, musicDal);
                menu.PostRun();

                if (number < 0)
                    break;
            }
        }
    }
}