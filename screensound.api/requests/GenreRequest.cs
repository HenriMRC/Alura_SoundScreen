using System.ComponentModel.DataAnnotations;

namespace screensound.api.requests;

public record GenreRequest([Required] string Name, string? Description);