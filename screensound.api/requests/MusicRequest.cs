using screensound.core.models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace screensound.api.requests;

public record MusicRequest([Required] string Name, [Required] int ArtistId,
    int? YearOfRelease, GenreRequest[]? Genres)
{
    public static implicit operator Music(MusicRequest music)
    {
        (string name, _, int? yearOfRelease, GenreRequest[]? genres) = music;
        Music output = new(name);
        if (yearOfRelease.HasValue)
            output.YearOfRelease = yearOfRelease.Value;
        output.Genres = genres?.Select(Selector).ToList() ?? [];
        static Genre Selector(GenreRequest g) => new(g.Name) { Description = g.Description };

        return output;
    }
}
