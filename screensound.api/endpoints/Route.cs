namespace screensound.api.endpoints;

public static class Route
{
    /// <summary>
    /// GET, POST and PUT
    /// </summary>
    public const string ARTISTS = "/artists";

    /// <summary>
    /// GET. <b>{0}</b> is the <b>name</b> of the artist<br></br>
    /// DELETE. <b>{0}</b> is the <b>id</b> of the artist
    /// </summary>
    public const string ARTIST_BY = $"{ARTISTS}/{{0}}";

    /// <summary>
    /// GET, POST and PUT
    /// </summary>
    public const string MUSICS = "/musics";

    /// <summary>
    /// GET. <b>{0}</b> is the <b>name</b> of the artist<br></br>
    /// DELETE. <b>{0}</b> is the <b>id</b> of the artist
    /// </summary>
    public const string MUSIC_BY = $"{MUSICS}/{{0}}";
}
