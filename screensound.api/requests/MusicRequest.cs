using screensound.core.models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace screensound.api.requests;

public record MusicRequest([Required] string Name, [Required] int ArtistId,
    int? YearOfRelease, GenreRequest[]? Genres);
