using screensound.core.models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace screensound.api.requests;

public record MusicRequest([Required] string Name, [Required] int ArtistId,
    int? YearOfRelease, GenreRequest[]? genres)
{
    public static implicit operator Music(MusicRequest music)
    {
        (string name, _, int? yearOfRelease, GenreRequest[]? genres) = music;
        Music output = new(name);
        if (yearOfRelease.HasValue)
            output.YearOfRelease = yearOfRelease.Value;
        output.Genres = genres?.Select(g => new Genre(g.Name) { Description = g.Description }).ToList()
                            ?? new List<Genre>();

        return output;
    }
}
