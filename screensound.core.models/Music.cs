namespace screensound.core.models
{
    public class Music
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int? YearOfRelease { get; set; }
        public virtual Artist? Artist { get; set; }

        public Music(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"\tId: {Id}\n\tName: {Name}\n\tArtist: {Artist?.Name ?? "[NOT REGISTERED]"}";
        }
    }
}