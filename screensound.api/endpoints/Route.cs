namespace screensound.api.endpoints;

public static class Routes
{
    /// <summary>
    /// GET, POST and PUT
    /// </summary>
    public const string ARTISTS = "/artists";
    /// <summary>
    /// GET, POST and PUT
    /// </summary>
    public static string GetUriArtists(string uri) => $"{uri}{ARTISTS}";

    /// <summary>
    /// GET. <b>{0}</b> is the <b>name</b> of the artist<br></br>
    /// DELETE. <b>{0}</b> is the <b>id</b> of the artist
    /// </summary>
    public const string ARTISTS_BY = $"{ARTISTS}/{{0}}";
    public static string GetRouteArtistsBy(string name) => $"{string.Format(ARTISTS_BY, name)}";
    public static string GetUriArtistsBy(string uri, string name) => $"{uri}{GetRouteArtistsBy(name)}";
    public static string GetRouteArtistsBy(int id) => $"{string.Format(ARTISTS_BY, id)}";
    public static string GetUriArtistsBy(string uri, int id) => $"{uri}{GetRouteArtistsBy(id)}";

    /// <summary>
    /// GET, POST and PUT
    /// </summary>
    public const string MUSICS = "/musics";
    public static string GetUriMusics(string uri) => $"{uri}{MUSICS}";

    /// <summary>
    /// GET. <b>{0}</b> is the <b>name</b> of the artist<br></br>
    /// DELETE. <b>{0}</b> is the <b>id</b> of the artist
    /// </summary>
    public const string MUSICS_BY = $"{MUSICS}/{{0}}";
    public static string GetRouteMusicsBy(string name) => $"{string.Format(MUSICS_BY, name)}";
    public static string GetUriMusicsBy(string uri, string name) => $"{uri}{GetRouteMusicsBy(name)}";
    public static string GetRouteMusicsBy(int id) => $"{string.Format(MUSICS_BY, id)}";
    public static string GetUriMusicsBy(string uri, int id) => $"{uri}{GetRouteMusicsBy(id)}";
}
