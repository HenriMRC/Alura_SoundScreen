using System.Collections.Generic;

namespace screensound.core.models;

public class Genre(string name)
{
    public int Id { get; init; }
    public string Name { get; set; } = name;
    public string? Description { get; set; }
    public virtual ICollection<Music> Musics { get; set; } = [];

    public override string ToString()
    {
        return $"Genre: {Name}\n{Description}";
    }
}
