using screensound.core.models;
using System.ComponentModel.DataAnnotations;

namespace screensound.api.requests;

public record MusicRequest([Required] string Name, [Required] int ArtistId, int? YearOfRelease)
{
    public static implicit operator Music(MusicRequest artist)
    {
        (string name, _, int? yearOfRelease) = artist;
        Music output = new(name);
        if (yearOfRelease.HasValue)
            output.YearOfRelease = yearOfRelease.Value;

        return output;
    }
}
