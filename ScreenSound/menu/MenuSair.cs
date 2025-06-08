using screensound.models;
using System;
using System.Collections.Generic;

namespace screensound.menu
{
    internal class MenuSair : Menu
    {
        public override void Executar(Dictionary<string, Artista> artistasRegistrados)
        {
            Console.WriteLine("Tchau tchau :)");
        }
    }
}
