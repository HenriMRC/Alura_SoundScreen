using screensound.database.dal;
using screensound.models;
using System;
using System.Collections.Generic;

namespace screensound.menu
{
    internal class ListMusicsOfYearMenu : Menu
    {
        public override string GetOptionInstruction(int optionIndex)
        {
            return $"Type {optionIndex} to print the musics of a year";
        }

        public override void Run(DAL<Artist> artistDal, DAL<Music> musicDal)
        {
            Console.Write("Type the musics' year of release: ");

            int yor;
            while (true)
            {
                string? yorStr = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(yorStr) && int.TryParse(yorStr, out yor))
                    break;

                Console.Write("Invalid music release year. Try again: ");
            }
            Console.WriteLine();

            List<Music> musics = musicDal.Where(m => m.YearOfRelease == yor);

            if (musics.Count == 0)
            {
                Console.WriteLine($"No music was release in {yor}");
            }
            else
            {
                Console.WriteLine($"Musics released in {yor}:\n");
                foreach (Music music in musics)
                    Console.WriteLine($"    {music}\n");
            }
        }
    }
}
