using System;

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
    public static Uri GetUriArtistsBy(string uri, string name) => new($"{uri}{GetRouteArtistsBy(name)}");
    public static string GetRouteArtistsBy(int id) => $"{string.Format(ARTISTS_BY, id)}";
    public static Uri GetUriArtistsBy(string uri, int id) => new($"{uri}{GetRouteArtistsBy(id)}");

    /// <summary>
    /// GET, POST and PUT
    /// </summary>
    public const string MUSICS = "/musics";
    public static string GetUriMusics(string uri) => $"{uri}{MUSICS}";

    /// <summary>
    /// GET. <b>{0}</b> is the <b>name</b> of the music<br></br>
    /// DELETE. <b>{0}</b> is the <b>id</b> of the music
    /// </summary>
    public const string MUSICS_BY = $"{MUSICS}/{{0}}";
    public static string GetRouteMusicsBy(string name) => $"{string.Format(MUSICS_BY, name)}";
    public static Uri GetUriMusicsBy(string uri, string name) => new($"{uri}{GetRouteMusicsBy(name)}");
    public static string GetRouteMusicsBy(int id) => $"{string.Format(MUSICS_BY, id)}";
    public static Uri GetUriMusicsBy(string uri, int id) => new($"{uri}{GetRouteMusicsBy(id)}");

    /// <summary>
    /// GET, POST and PUT
    /// </summary>
    public const string GENRES = "/genres";
    public static string GetUriGenres(string uri) => $"{uri}{GENRES}";

    /// <summary>
    /// GET. <b>{0}</b> is the <b>name</b> of the genre<br></br>
    /// DELETE. <b>{0}</b> is the <b>id</b> of the genre
    /// </summary>
    public const string GENRES_BY = $"{GENRES}/{{0}}";
    public static string GetUriGenresBy(string name) => $"{string.Format(GENRES_BY, name)}";
    public static Uri GetUriGenresBy(string uri, string name) => new($"{uri}{GetUriGenresBy(name)}");
    public static string GetUriGenresBy(int id) => $"{string.Format(GENRES_BY, id)}";
    public static Uri GetUriGenresBy(string uri, int id) => new($"{uri}{GetUriGenresBy(id)}");

}
