using System.ComponentModel.DataAnnotations;

namespace screensound.api.requests;

public record UpdateArtistRequest([Required] int Id, string? Name, string? Bio, string? ProfileImage);