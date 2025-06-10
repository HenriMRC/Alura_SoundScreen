using screensound.database;
using screensound.database.dal;
using screensound.models;
using System;

namespace screensound.menu
{
    internal class ExitMenu : Menu
    {
        public override void Executar(DAL<Artist> dal)
        {
            Console.WriteLine("Bye bye :)");
        }
    }
}
