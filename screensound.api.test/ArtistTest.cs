using screensound.api.endpoints;
using screensound.api.requests;
using screensound.api.responses;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

using Artist = screensound.core.models.Artist;

namespace screensound.api.test;

public class ArtistTest : BaseTest
{
    protected override string DbName => DB_NAME;
    private const string DB_NAME = "ScreenSoundTest";

    [Test]
    public void Test()
    {
        using HttpClient client = new();

        const string BIO = "Metallica is an American heavy metal band. It was formed in Los Angeles in 1981 by vocalist and guitarist James Hetfield and drummer Lars Ulrich, and has been based in San Francisco for most of its career.";
        const int EXPECTED_ID = 1;
        {
            const string WRONG_NAME = "Metalica";
            HttpContent content = JsonContent.Create(new ArtistRequest(WRONG_NAME, BIO, null));
            HttpResponseMessage result = client.PostAsync(Routes.GetUriArtists(Uri), content).Result;
            Assert.Multiple(() =>
            {
                Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(result.Headers.Location?.OriginalString, Is.EqualTo(Routes.GetRouteArtistsBy(WRONG_NAME)));
            });
            string resultContent = result.Content.ReadAsStringAsync().Result;
            ArtistResponse? artist = JsonSerializer.Deserialize<ArtistResponse>(resultContent, JsonSerializerOptions.Web);
            Assert.That(artist, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(artist.Name, Is.EqualTo(WRONG_NAME));
                Assert.That(artist.Bio, Is.EqualTo(BIO));
                Assert.That(artist.ProfileImage, Is.EqualTo(Artist.DEFAULT_PROFILE_IMAGE));
                Assert.That(artist.Id, Is.EqualTo(EXPECTED_ID));
                CollectionAssert.AreEqual(artist.Musics, Array.Empty<ArtistResponse.MusicData>());
            });
        }

        const string RIGHT_NAME = "Metallica";
        {
            JsonContent content = JsonContent.Create(new UpdateArtistRequest(1, RIGHT_NAME, null, null));
            HttpResponseMessage result = client.PutAsync(Routes.GetUriArtists(Uri), content).Result;
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            string resultContent = result.Content.ReadAsStringAsync().Result;
            ArtistResponse? artist = JsonSerializer.Deserialize<ArtistResponse>(resultContent, JsonSerializerOptions.Web);
            Assert.That(artist, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(artist.Name, Is.EqualTo(RIGHT_NAME));
                Assert.That(artist.Bio, Is.EqualTo(BIO));
                Assert.That(artist.ProfileImage, Is.EqualTo(Artist.DEFAULT_PROFILE_IMAGE));
                Assert.That(artist.Id, Is.EqualTo(EXPECTED_ID));
                CollectionAssert.AreEqual(artist.Musics, Array.Empty<ArtistResponse.MusicData>());
            });
        }

        {
            HttpResponseMessage result = client.GetAsync(Routes.GetUriArtists(Uri)).Result;
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            string resultContent = result.Content.ReadAsStringAsync().Result;
            ArtistResponse[]? artists = JsonSerializer.Deserialize<ArtistResponse[]>(resultContent, JsonSerializerOptions.Web);
            Assert.That(artists, Is.Not.Null);
            Assert.That(artists, Has.Length.EqualTo(1));
            ArtistResponse artist = artists[0];
            Assert.Multiple(() =>
            {
                Assert.That(artist.Name, Is.EqualTo(RIGHT_NAME));
                Assert.That(artist.Bio, Is.EqualTo(BIO));
                Assert.That(artist.ProfileImage, Is.EqualTo(Artist.DEFAULT_PROFILE_IMAGE));
                Assert.That(artist.Id, Is.EqualTo(EXPECTED_ID));
                CollectionAssert.AreEqual(artist.Musics, Array.Empty<ArtistResponse.MusicData>());
            });
        }

        {
            HttpResponseMessage result = client.GetAsync(Routes.GetUriArtistsBy(Uri, RIGHT_NAME)).Result;
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            string resultContent = result.Content.ReadAsStringAsync().Result;
            ArtistResponse? artist = JsonSerializer.Deserialize<ArtistResponse>(resultContent, JsonSerializerOptions.Web);
            Assert.That(artist, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(artist.Name, Is.EqualTo(RIGHT_NAME));
                Assert.That(artist.Bio, Is.EqualTo(BIO));
                Assert.That(artist.ProfileImage, Is.EqualTo(Artist.DEFAULT_PROFILE_IMAGE));
                Assert.That(artist.Id, Is.EqualTo(EXPECTED_ID));
                CollectionAssert.AreEqual(artist.Musics, Array.Empty<ArtistResponse.MusicData>());
            });
        }

        {
            HttpResponseMessage result = client.DeleteAsync(Routes.GetUriArtistsBy(Uri, EXPECTED_ID)).Result;
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        {
            HttpResponseMessage result = client.GetAsync(Routes.GetUriArtists(Uri)).Result;
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            string resultContent = result.Content.ReadAsStringAsync().Result;
            ArtistResponse[]? artists = JsonSerializer.Deserialize<ArtistResponse[]>(resultContent, JsonSerializerOptions.Web);
            Assert.That(artists, Is.Not.Null);
            Assert.That(artists, Has.Length.EqualTo(0));
        }
    }
}
