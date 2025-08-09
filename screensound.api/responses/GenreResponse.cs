using screensound.core.models;

namespace screensound.api.responses;

public record GenreResponse(int Id, string Name, string? Description)
{
    public static implicit operator GenreResponse(Genre genre)
    {
        return new(genre.Id, genre.Name, genre.Description/*, genre.Musics*/); //TODO
    }
}