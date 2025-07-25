using System.ComponentModel.DataAnnotations;

namespace screensound.api.requests;

public record UpdateMusicRequest([Required] int Id, string? Name, int? YearOfRelease, int? ArtistId);