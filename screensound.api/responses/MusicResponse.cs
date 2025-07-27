using screensound.core.models;

namespace screensound.api.responses;

public record MusicResponse(int Id, string Name, int? YearOfRelease, MusicResponse.ArtistData? Artist)
{
    public static implicit operator MusicResponse(Music music)
    {
        return new(music.Id, music.Name, music.YearOfRelease, music.Artist);
    }

    public record ArtistData(int Id, string Name, string ProfileImage, string Bio)
    {
        public static implicit operator ArtistData?(Artist? artist)
        {
            if (artist == null)
                return null;
            return new(artist.Id, artist.Name, artist.ProfileImage, artist.Bio);
        }
    }
}
