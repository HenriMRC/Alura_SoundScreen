using System;
using System.Collections.Generic;

namespace screensound.models
{
    public class Artist
    {
        private readonly List<Music> _musics = new();

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

        public void AddMusic(Music musica)
        {
            _musics.Add(musica);
        }

        public void ShowDiscography()
        {
            Console.WriteLine($"Artist's dircography {Name}");
            foreach (var musica in _musics)
            {
                Console.WriteLine($"Music: {musica.Name}");
            }
        }

        public override string ToString()
        {
            return 
$@"            Id: {Id}
            Name: {Name}
            Profile image: {ProfileImage}
            Bio: {Bio}";
        }
    }
}