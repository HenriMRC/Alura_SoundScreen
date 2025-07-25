using screensound.core.models;
using System.ComponentModel.DataAnnotations;

namespace screensound.api.requests;

public record ArtistRequest([Required] string Name, [Required] string Bio, string? ProfileImage)
{
    public static implicit operator Artist(ArtistRequest artist)
    {
        (string name, string bio, string? image) = artist;
        Artist output = new(name, bio);
        if (!string.IsNullOrWhiteSpace(image))
            output.ProfileImage = image;

        return output;
    }
}
