using screensound.database.dal;
using screensound.core.models;
using System;
using System.Threading;

namespace screensound.menu
{
    internal class ExitMenu : Menu
    {
        public override string GetOptionInstruction(int optionIndex)
        {
            return $"Type {optionIndex} to exit";
        }

        public override void Run(DAL<Artist> artistDal, DAL<Music> musicDal)
        {
            Console.WriteLine("Bye bye :)");
        }

        public override void PostRun()
        {
            Thread.Sleep(4_000);
        }
    }
}
