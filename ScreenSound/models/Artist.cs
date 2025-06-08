using System;
using System.Collections.Generic;

namespace screensound.models
{
    internal class Artist
    {
        private readonly List<Musica> _musicas = new();

        public Artist(string name, string bio)
        {
            Name = name;
            Bio = bio;
            ProfileImage = "https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_1280.png";
        }

        public string Name { get; set; }
        public string ProfileImage { get; set; }
        public string Bio { get; set; }
        public int Id { get; set; }

        public void AdicionarMusica(Musica musica)
        {
            _musicas.Add(musica);
        }

        public void ExibirDiscografia()
        {
            Console.WriteLine($"Discografia do artista {Name}");
            foreach (var musica in _musicas)
            {
                Console.WriteLine($"Música: {musica.Nome}");
            }
        }

        public override string ToString()
        {
            return 
$@"            Id: {Id}
            Nome: {Name}
            Foto de Perfil: {ProfileImage}
            Bio: {Bio}";
        }
    }
}