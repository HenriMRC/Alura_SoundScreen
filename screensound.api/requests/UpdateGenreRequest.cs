using System.ComponentModel.DataAnnotations;

namespace screensound.api.requests;

public record UpdateGenreRequest([Required] int Id, string? Name, string? Description);