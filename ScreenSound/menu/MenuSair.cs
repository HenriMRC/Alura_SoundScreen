using screensound.database;
using System;

namespace screensound.menu
{
    internal class MenuSair : Menu
    {
        public override void Executar(ArtistDAL dal)
        {
            Console.WriteLine("Bye bye :)");
        }
    }
}
