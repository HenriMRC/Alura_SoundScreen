using System;
using System.Collections.Generic;

namespace screensound.models
{
    public class Artist
    {
        public string Name { get; set; }
        public string ProfileImage { get; set; }
        public string Bio { get; set; }
        public int Id { get; set; }
        public virtual ICollection<Music> Musics { get; set; } = new List<Music>();

        public Artist(string name, string bio)
        {
            Name = name;
            Bio = bio;
            ProfileImage = "https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_1280.png";
        }

        public void AddMusic(Music musica)
        {
            Musics.Add(musica);
        }

        public void ShowDiscography()
        {
            Console.WriteLine($"Artist's dircography {Name}");
            foreach (var musica in Musics)
            {
                Console.WriteLine($"Music: {musica.Name} - Year: {musica.YearOfRelease}");
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