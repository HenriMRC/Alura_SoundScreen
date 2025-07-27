using screensound.api.requests;
using screensound.core.models;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using static screensound.api.endpoints.Route;


namespace screensound.api.test;

public class ArtistTest : BaseTest
{
    protected override string DbName => DB_NAME;
    private const string DB_NAME = "ScreenSoundTest";

    [Test]
    public void Test()
    {
        using HttpClient client = new();

        const string WRONG_NAME = "Metalica";
        const string BIO = "Metallica is an American heavy metal band. It was formed in Los Angeles in 1981 by vocalist and guitarist James Hetfield and drummer Lars Ulrich, and has been based in San Francisco for most of its career.";
        HttpContent content = JsonContent.Create(new ArtistRequest(WRONG_NAME, BIO, null));
        HttpResponseMessage result = client.PostAsync(Url + ARTISTS, content).Result;
        Assert.Multiple(() =>
        {
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(result.Headers.Location?.OriginalString, Is.EqualTo(string.Format(ARTIST_BY, WRONG_NAME)));
        });
        string resultContent = result.Content.ReadAsStringAsync().Result;
        Artist? artist = JsonSerializer.Deserialize<Artist>(resultContent, JsonSerializerOptions.Web);
        Assert.That(artist, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(artist.Name, Is.EqualTo(WRONG_NAME));
            Assert.That(artist.Bio, Is.EqualTo(BIO));
            Assert.That(artist.ProfileImage, Is.EqualTo(Artist.DEFAULT_PROFILE_IMAGE));
            Assert.That(artist.Id, Is.EqualTo(1));
            CollectionAssert.AreEqual(artist.Musics, Array.Empty<Music>());
        });

        const string RIGHT_NAME = "Metallica";
        content = JsonContent.Create(new UpdateArtistRequest(1, RIGHT_NAME, null, null));
        result = client.PutAsync(Url + ARTISTS, content).Result;
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        resultContent = result.Content.ReadAsStringAsync().Result;
        artist = JsonSerializer.Deserialize<Artist>(resultContent, JsonSerializerOptions.Web);
        Assert.That(artist, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(artist.Name, Is.EqualTo(RIGHT_NAME));
            Assert.That(artist.Bio, Is.EqualTo(BIO));
            Assert.That(artist.ProfileImage, Is.EqualTo(Artist.DEFAULT_PROFILE_IMAGE));
            Assert.That(artist.Id, Is.EqualTo(1));
            CollectionAssert.AreEqual(artist.Musics, Array.Empty<Music>());
        });

        result = client.GetAsync(Url + ARTISTS).Result;
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        resultContent = result.Content.ReadAsStringAsync().Result;
        Artist[]? artists = JsonSerializer.Deserialize<Artist[]>(resultContent, JsonSerializerOptions.Web);
        Assert.That(artists, Is.Not.Null);
        Assert.That(artists, Has.Length.EqualTo(1));
        artist = artists[0];
        Assert.Multiple(() =>
        {
            Assert.That(artist.Name, Is.EqualTo(RIGHT_NAME));
            Assert.That(artist.Bio, Is.EqualTo(BIO));
            Assert.That(artist.ProfileImage, Is.EqualTo(Artist.DEFAULT_PROFILE_IMAGE));
            Assert.That(artist.Id, Is.EqualTo(1));
            CollectionAssert.AreEqual(artist.Musics, Array.Empty<Music>());
        });

        const int EXPECTED_ID = 1;
        result = client.GetAsync(Url + string.Format(ARTIST_BY, RIGHT_NAME)).Result;
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        resultContent = result.Content.ReadAsStringAsync().Result;
        artist = JsonSerializer.Deserialize<Artist>(resultContent, JsonSerializerOptions.Web);
        Assert.That(artist, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(artist.Name, Is.EqualTo(RIGHT_NAME));
            Assert.That(artist.Bio, Is.EqualTo(BIO));
            Assert.That(artist.ProfileImage, Is.EqualTo(Artist.DEFAULT_PROFILE_IMAGE));
            Assert.That(artist.Id, Is.EqualTo(EXPECTED_ID));
            CollectionAssert.AreEqual(artist.Musics, Array.Empty<Music>());
        });

        result = client.DeleteAsync(Url + string.Format(ARTIST_BY, EXPECTED_ID)).Result;
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

        result = client.GetAsync(Url + ARTISTS).Result;
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        resultContent = result.Content.ReadAsStringAsync().Result;
        artists = JsonSerializer.Deserialize<Artist[]>(resultContent, JsonSerializerOptions.Web);
        Assert.That(artists, Is.Not.Null);
        Assert.That(artists, Has.Length.EqualTo(0));
    }
}
