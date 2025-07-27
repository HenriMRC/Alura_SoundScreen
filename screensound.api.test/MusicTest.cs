using screensound.api.requests;
using screensound.core.models;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

using static screensound.api.endpoints.Route;

namespace screensound.api.test;

internal class MusicTest : BaseTest
{
    protected override string DbName => DB_NAME;
    private const string DB_NAME = "ScreenSoundTest";

    private const string METALLICA = "Metallica";
    private const string METALLICA_BIO = "Metallica is an American heavy metal band. It was formed in Los Angeles in 1981 by vocalist and guitarist James Hetfield and drummer Lars Ulrich, and has been based in San Francisco for most of its career.";
    private const int METALLICA_ID = 1;

    private const string LINKIN_PARK = "Linkin Park";
    private const string LINKIN_PARK_BIO = "Linkin Park is an American rock band formed in Agoura Hills, California, in 1996.";
    private const int LINKIN_PARK_ID = 2;

    [OneTimeSetUp]
    public override void OneTimeSetUp()
    {
        base.OneTimeSetUp();

        Artist artist = new(METALLICA, METALLICA_BIO);
        Context.Artists.Add(artist);
        
        artist = new(LINKIN_PARK, LINKIN_PARK_BIO);
        Context.Artists.Add(artist);

        Context.SaveChanges();
    }

    [Test]
    public void Test()
    {
        using HttpClient client = new();

        #region Master of Puppets
        const string MUSIC_WRONG_NAME = "Master of Pupets";
        const int YEAR_OF_RELEASE = 1986;
        JsonContent content = JsonContent.Create(new MusicRequest(MUSIC_WRONG_NAME, METALLICA_ID, YEAR_OF_RELEASE));
        HttpResponseMessage result = client.PostAsync(Url + MUSICS, content).Result;
        Assert.Multiple(() =>
        {
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(result.Headers.Location?.OriginalString, Is.EqualTo(string.Format(MUSIC_BY, MUSIC_WRONG_NAME)));
        });
        string resultContent = result.Content.ReadAsStringAsync().Result;
        Music? music = JsonSerializer.Deserialize<Music>(resultContent, JsonSerializerOptions.Web);
        Assert.That(music, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(music.Name, Is.EqualTo(MUSIC_WRONG_NAME));
            Assert.That(music.YearOfRelease, Is.EqualTo(YEAR_OF_RELEASE));
            Assert.That(music.Id, Is.EqualTo(1));
        });

        Artist? artist = music.Artist;
        Assert.That(artist, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(artist.Name, Is.EqualTo(METALLICA));
            Assert.That(artist.Bio, Is.EqualTo(METALLICA_BIO));
            Assert.That(artist.ProfileImage, Is.EqualTo(Artist.DEFAULT_PROFILE_IMAGE));
            Assert.That(artist.Id, Is.EqualTo(METALLICA_ID));
            CollectionAssert.AreEqual(artist.Musics, new Music?[] { null });
        });

        result = client.GetAsync(Url + string.Format(ARTIST_BY, METALLICA)).Result;
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        resultContent = result.Content.ReadAsStringAsync().Result;
        artist = JsonSerializer.Deserialize<Artist>(resultContent, JsonSerializerOptions.Web);
        Assert.That(artist, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(artist.Name, Is.EqualTo(METALLICA));
            Assert.That(artist.Bio, Is.EqualTo(METALLICA_BIO));
            Assert.That(artist.ProfileImage, Is.EqualTo(Artist.DEFAULT_PROFILE_IMAGE));
            Assert.That(artist.Id, Is.EqualTo(METALLICA_ID));

            Assert.That(artist.Musics, Has.Count.EqualTo(1));
        });

        music = artist.Musics.First();
        Assert.Multiple(() =>
        {
            Assert.That(music.Name, Is.EqualTo(MUSIC_WRONG_NAME));
            Assert.That(music.YearOfRelease, Is.EqualTo(YEAR_OF_RELEASE));
            Assert.That(music.Id, Is.EqualTo(1));
        });
        Assert.That(music.Artist, Is.Null);

        const string MUSIC_RIGHT_NAME = "Master of Puppets";
        content = JsonContent.Create(new UpdateMusicRequest(1, MUSIC_RIGHT_NAME, null, null));
        result = client.PutAsync(Url + MUSICS, content).Result;
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        resultContent = result.Content.ReadAsStringAsync().Result;
        music = JsonSerializer.Deserialize<Music>(resultContent, JsonSerializerOptions.Web);
        Assert.That(music, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(music.Name, Is.EqualTo(MUSIC_RIGHT_NAME));
            Assert.That(music.YearOfRelease, Is.EqualTo(YEAR_OF_RELEASE));
            Assert.That(music.Id, Is.EqualTo(1));
        });

        artist = music.Artist;
        Assert.That(artist, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(artist.Name, Is.EqualTo(METALLICA));
            Assert.That(artist.Bio, Is.EqualTo(METALLICA_BIO));
            Assert.That(artist.ProfileImage, Is.EqualTo(Artist.DEFAULT_PROFILE_IMAGE));
            Assert.That(artist.Id, Is.EqualTo(METALLICA_ID));
            CollectionAssert.AreEqual(artist.Musics, new Music?[] { null });
        });

        result = client.GetAsync(Url + MUSICS).Result;
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        resultContent = result.Content.ReadAsStringAsync().Result;
        Music[]? musics = JsonSerializer.Deserialize<Music[]>(resultContent, JsonSerializerOptions.Web);
        Assert.That(musics, Is.Not.Null);
        Assert.That(musics, Has.Length.EqualTo(1));
        music = musics[0];
        Assert.That(music, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(music.Name, Is.EqualTo(MUSIC_RIGHT_NAME));
            Assert.That(music.YearOfRelease, Is.EqualTo(YEAR_OF_RELEASE));
            Assert.That(music.Id, Is.EqualTo(1));
        });

        artist = music.Artist;
        Assert.That(artist, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(artist.Name, Is.EqualTo(METALLICA));
            Assert.That(artist.Bio, Is.EqualTo(METALLICA_BIO));
            Assert.That(artist.ProfileImage, Is.EqualTo(Artist.DEFAULT_PROFILE_IMAGE));
            Assert.That(artist.Id, Is.EqualTo(1));
            CollectionAssert.AreEqual(artist.Musics, new Music?[] { null });
        });

        result = client.GetAsync(Url + string.Format(MUSIC_BY, MUSIC_RIGHT_NAME)).Result;
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        resultContent = result.Content.ReadAsStringAsync().Result;
        musics = JsonSerializer.Deserialize<Music[]>(resultContent, JsonSerializerOptions.Web);
        Assert.That(musics, Is.Not.Null);
        Assert.That(musics, Has.Length.EqualTo(1));
        music = musics[0];
        Assert.That(music, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(music.Name, Is.EqualTo(MUSIC_RIGHT_NAME));
            Assert.That(music.YearOfRelease, Is.EqualTo(YEAR_OF_RELEASE));
            Assert.That(music.Id, Is.EqualTo(1));
        });

        artist = music.Artist;
        Assert.That(artist, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(artist.Name, Is.EqualTo(METALLICA));
            Assert.That(artist.Bio, Is.EqualTo(METALLICA_BIO));
            Assert.That(artist.ProfileImage, Is.EqualTo(Artist.DEFAULT_PROFILE_IMAGE));
            Assert.That(artist.Id, Is.EqualTo(METALLICA_ID));
            CollectionAssert.AreEqual(artist.Musics, new Music?[] { null });
        });

        result = client.DeleteAsync(Url + string.Format(MUSIC_BY, 1)).Result;
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

        result = client.GetAsync(Url + MUSICS).Result;
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        resultContent = result.Content.ReadAsStringAsync().Result;
        musics = JsonSerializer.Deserialize<Music[]>(resultContent, JsonSerializerOptions.Web);
        Assert.That(musics, Is.Not.Null);
        Assert.That(musics, Has.Length.EqualTo(0));
        #endregion

        #region In the End
        const string IN_THE_END = "In the end";
        const int IN_THE_END_YOR = 2001;
        content = JsonContent.Create(new MusicRequest(IN_THE_END, METALLICA_ID, IN_THE_END_YOR));
        result = client.PostAsync(Url + MUSICS, content).Result;
        Assert.Multiple(() =>
        {
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(result.Headers.Location?.OriginalString, Is.EqualTo(string.Format(MUSIC_BY, IN_THE_END)));
        });
        resultContent = result.Content.ReadAsStringAsync().Result;
        music = JsonSerializer.Deserialize<Music>(resultContent, JsonSerializerOptions.Web);
        Assert.That(music, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(music.Name, Is.EqualTo(IN_THE_END));
            Assert.That(music.YearOfRelease, Is.EqualTo(IN_THE_END_YOR));
            Assert.That(music.Id, Is.EqualTo(2));
        });

        artist = music.Artist;
        Assert.That(artist, Is.Not.Null);
        Assert.That(artist.Id, Is.EqualTo(METALLICA_ID));

        content = JsonContent.Create(new UpdateMusicRequest(2, null, null, LINKIN_PARK_ID));
        result = client.PutAsync(Url + MUSICS, content).Result;
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        resultContent = result.Content.ReadAsStringAsync().Result;
        music = JsonSerializer.Deserialize<Music>(resultContent, JsonSerializerOptions.Web);
        Assert.That(music, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(music.Name, Is.EqualTo(IN_THE_END));
            Assert.That(music.YearOfRelease, Is.EqualTo(IN_THE_END_YOR));
            Assert.That(music.Id, Is.EqualTo(2));
        });

        Assert.That(music.Artist, Is.Not.Null);
        Assert.That(music.Artist.Id, Is.EqualTo(LINKIN_PARK_ID));

        result = client.DeleteAsync(Url + string.Format(MUSIC_BY, 2)).Result;
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        #endregion
    }

    [Test]
    public void TestWithArtistIdThatDoesNotExists()
    {
        using HttpClient client = new();

        const string NUMB = "Numb";
        const int NUMB_YOR = 2003;
        JsonContent content = JsonContent.Create(new MusicRequest(NUMB, 1_000, NUMB_YOR));
        HttpResponseMessage result = client.PostAsync(Url + MUSICS, content).Result;
        string resultContent = result.Content.ReadAsStringAsync().Result;
        Assert.Multiple(() =>
        {
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(resultContent, Is.EqualTo($"\"Artist {1_000} not found\""));
        });
    }
}
