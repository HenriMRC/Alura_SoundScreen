using screensound.api.endpoints;
using screensound.api.requests;
using screensound.api.responses;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace screensound.api.test;

internal class GenreTest : BaseTest
{
    protected override string DbName => DB_NAME;
    private const string DB_NAME = "ScreenSoundTest";

    [Test]
    public void Test()
    {
        using HttpClient client = new();

        const string GENRE = "Rock";
        const int EXPECTED_ID = 1;
        {
            HttpContent content = JsonContent.Create(new GenreRequest(GENRE, null));
            HttpResponseMessage result = client.PostAsync(Routes.GetUriGenres(Uri), content).Result;
            Assert.Multiple(() =>
            {
                Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(result.Headers.Location?.OriginalString, Is.EqualTo(Routes.GetUriGenresBy(GENRE)));
            });
            string resultContent = result.Content.ReadAsStringAsync().Result;
            GenreResponse? genre = JsonSerializer.Deserialize<GenreResponse>(resultContent, JsonSerializerOptions.Web);
            Assert.That(genre, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(genre.Name, Is.EqualTo(GENRE));
                Assert.That(genre.Id, Is.EqualTo(EXPECTED_ID));
                Assert.That(genre.Description, Is.Null);
            });
        }

        const string DESCRITION = "The best genre";
        {
            JsonContent content = JsonContent.Create(new UpdateGenreRequest(1, null, DESCRITION));
            HttpResponseMessage result = client.PutAsync(Routes.GetUriGenres(Uri), content).Result;
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            string resultContent = result.Content.ReadAsStringAsync().Result;
            GenreResponse? genre = JsonSerializer.Deserialize<GenreResponse>(resultContent, JsonSerializerOptions.Web);
            Assert.That(genre, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(genre.Name, Is.EqualTo(GENRE));
                Assert.That(genre.Id, Is.EqualTo(EXPECTED_ID));
                Assert.That(genre.Description, Is.EqualTo(DESCRITION));
            });
        }

        {
            HttpResponseMessage result = client.GetAsync(Routes.GetUriGenres(Uri)).Result;
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            string resultContent = result.Content.ReadAsStringAsync().Result;
            GenreResponse[]? genres = JsonSerializer.Deserialize<GenreResponse[]>(resultContent, JsonSerializerOptions.Web);
            Assert.That(genres, Is.Not.Null);
            Assert.That(genres, Has.Length.EqualTo(1));
            GenreResponse genre = genres[0];
            Assert.Multiple(() =>
            {
                Assert.That(genre.Name, Is.EqualTo(GENRE));
                Assert.That(genre.Id, Is.EqualTo(EXPECTED_ID));
                Assert.That(genre.Description, Is.EqualTo(DESCRITION));
            });
        }

        {
            HttpResponseMessage result = client.GetAsync(Routes.GetUriGenresBy(Uri, GENRE)).Result;
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            string resultContent = result.Content.ReadAsStringAsync().Result;
            GenreResponse[]? genres = JsonSerializer.Deserialize<GenreResponse[]>(resultContent, JsonSerializerOptions.Web);
            Assert.That(genres, Is.Not.Null);
            Assert.That(genres, Has.Length.EqualTo(1));
            GenreResponse genre = genres[0];
            Assert.Multiple(() =>
            {
                Assert.That(genre.Name, Is.EqualTo(GENRE));
                Assert.That(genre.Id, Is.EqualTo(EXPECTED_ID));
                Assert.That(genre.Description, Is.EqualTo(DESCRITION));
            });
        }

        {
            HttpResponseMessage result = client.DeleteAsync(Routes.GetUriGenresBy(Uri, EXPECTED_ID)).Result;
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        {
            HttpResponseMessage result = client.GetAsync(Routes.GetUriGenres(Uri)).Result;
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            string resultContent = result.Content.ReadAsStringAsync().Result;
            GenreResponse[]? genres = JsonSerializer.Deserialize<GenreResponse[]>(resultContent, JsonSerializerOptions.Web);
            Assert.That(genres, Is.Not.Null);
            Assert.That(genres, Has.Length.EqualTo(0));
        }
    }
}
