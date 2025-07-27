using screensound.core.models;
using System.Linq;

namespace screensound.api.responses;

public record ArtistResponse(int Id, string Name, string ProfileImage, string Bio, ArtistResponse.MusicData[] Musics)
{
    public static implicit operator ArtistResponse(Artist artist)
    {
        return new(artist.Id, artist.Name, artist.ProfileImage, artist.Bio, [.. artist.Musics.Select(m => (MusicData)m)]);
    }

    public record MusicData(int Id, string Name, int? YearOfRelease)
    {
        public static implicit operator MusicData(Music music)
        {
            return new(music.Id, music.Name, music.YearOfRelease);
        }
    }
}
