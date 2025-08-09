using screensound.core.models;
using System.Linq;
using System.Text.Json.Serialization;

namespace screensound.api.responses;

public record MusicResponse(int Id, string Name, int? YearOfRelease,
    MusicResponse.ArtistData? Artist, MusicResponse.GenreData[] Genres)
{
    public static implicit operator MusicResponse(Music music)
    {
        return new(music.Id, music.Name, music.YearOfRelease, music.Artist,
            music.Genres?.Select(g => (GenreData)g).ToArray() ?? []);
    }

    public record ArtistData(int Id, string Name, string ProfileImage, string Bio)
    {
        public static implicit operator ArtistData?(Artist? artist)
        {
            if (artist == null)
                return null;
            return new(artist.Id, artist.Name, artist.ProfileImage, artist.Bio);
        }
    }

    public record GenreData(int Id, string Name, string? Description)
    {
        public int Id { get; init; } = Id;
        public string Name { get; init; } = Name;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Description { get; init; } = Description;

        public GenreData(int id, string name) : this(id, name, null) { }

        public static implicit operator GenreData(Genre genre)
        {
            return new(genre.Id, genre.Name, genre.Description);
        }
    }
}