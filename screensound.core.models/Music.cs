namespace screensound.core.models;

public class Music(string name)
{
    public string Name { get; set; } = name;
    public int Id { get; init; }
    public int? YearOfRelease { get; set; }
    public virtual Artist? Artist { get; set; }

    public Music() : this(string.Empty) { }

    public override string ToString()
    {
        return $"\tId: {Id}\n\tName: {Name}\n\tArtist: {Artist?.Name ?? "[NOT REGISTERED]"}";
    }
}