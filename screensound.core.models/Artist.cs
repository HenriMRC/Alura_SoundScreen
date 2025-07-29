using System;
using System.Collections.Generic;

namespace screensound.core.models;

public class Artist(string name, string bio)
{
    public string Name { get; set; } = name;
    public string ProfileImage { get; set; } = DEFAULT_PROFILE_IMAGE;
    public string Bio { get; set; } = bio;
    public int Id { get; init; }
    public virtual ICollection<Music> Musics { get; set; } = [];
    public const string DEFAULT_PROFILE_IMAGE = "https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_1280.png";

    public Artist() : this(string.Empty, string.Empty) { }

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