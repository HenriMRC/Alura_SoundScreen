using screensound.core.models;
using System.ComponentModel.DataAnnotations;

namespace screensound.api.requests;

public record GenreRequest([Required] string Name, string? Description)
{
    public static implicit operator Genre(GenreRequest genre)
    {
        (string name, string? description) = genre;
        return new Genre(name) { Description = description };
    }
}