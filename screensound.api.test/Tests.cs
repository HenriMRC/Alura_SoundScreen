using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using screensound.core.models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

using static screensound.api.endpoints.Route;

namespace screensound.api.test;

public class Tests
{
    private const string SERVER_CONNECTION_STRING =
    "Data Source=(localdb)\\MSSQLLocalDB;" +
    "Initial Catalog=master;" +
    "Integrated Security=True;" +
    "Connect Timeout=30;" +
    "Encrypt=False;" +
    "Trust Server Certificate=False;" +
    "Application Intent=ReadWrite;" +
    "Multi Subnet Failover=False";

    private const string SSTEST_CONNECTION_STRING =
        "Data Source=(localdb)\\MSSQLLocalDB;" +
        $"Initial Catalog={SSTEST_NAME};" +
        "Integrated Security=True;" +
        "Connect Timeout=30;" +
        "Encrypt=False;" +
        "Trust Server Certificate=False;" +
        "Application Intent=ReadWrite;" +
        "Multi Subnet Failover=False";

    private const string SSTEST_NAME = "ScreenSoundTest";

    private SqlConnection _serverConnection;
    private IDisposable _webApp;
    private string _url;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _serverConnection = new(SERVER_CONNECTION_STRING);
        _serverConnection.Open();

        // ===== Create DB =====
        const string CREATE_SSTEST = $"CREATE DATABASE {SSTEST_NAME}";
        using SqlCommand createSsTestCommand = new(CREATE_SSTEST, _serverConnection);
        createSsTestCommand.ExecuteNonQuery();

        using SqlConnection dbConnection = new(SSTEST_CONNECTION_STRING);
        dbConnection.Open();

        // ===== Create Musics Table =====
        const string CREATE_MUSIC_TABLE =
            """
                CREATE TABLE Musics(
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        Name NVARCHAR(255) NOT NULL,
                        YearOfRelease INT NULL,
                        ArtistId INT NULL)
                """;
        using SqlCommand createMusicDbCommand = new(CREATE_MUSIC_TABLE, dbConnection);
        createMusicDbCommand.ExecuteNonQuery();

        const string CREATE_INDEX_MUSIC_TABLE =
            """
                CREATE INDEX IX_Musics_ArtistId
                ON Musics (ArtistId);
                """;
        using SqlCommand createIndexMusicDbCommand = new(CREATE_INDEX_MUSIC_TABLE, dbConnection);
        createIndexMusicDbCommand.ExecuteNonQuery();

        // ===== Create Artists Table =====
        const string CREATE_ARTISTS_TABLE =
            """
                CREATE TABLE Artists(
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        "Name" NVARCHAR(255) NOT NULL,
                        Bio NVARCHAR(255) NOT NULL,
                        ProfileImage NVARCHAR(255) NOT NULL
                );
                """;
        using SqlCommand createArtistsDbCommand = new(CREATE_ARTISTS_TABLE, dbConnection);
        createArtistsDbCommand.ExecuteNonQuery();

        // ===== Create Musics FK =====
        const string CREATE_FK_MUSIC_TABLE =
            """
                ALTER TABLE Musics
                	ADD CONSTRAINT FK_Musics_Artists_ArtistId
                		FOREIGN KEY (ArtistId) REFERENCES Artists (Id);
                """;
        using SqlCommand createFkMusicDbCommand = new(CREATE_FK_MUSIC_TABLE, dbConnection);
        createFkMusicDbCommand.ExecuteNonQuery();

        WebApplication webApp = Program.GetApp([], DbContextAction);
        static void DbContextAction(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(SSTEST_CONNECTION_STRING);
        }

        _webApp = webApp;
        Task startupTask = webApp.StartAsync();
        startupTask.Wait();
        _url = webApp.Urls.First();
    }

    [Test]
    public void Test()
    {
        using HttpClient client = new();

        const string WRONG_NAME = "Metalica";
        const string BIO = "Metallica is an American heavy metal band. It was formed in Los Angeles in 1981 by vocalist and guitarist James Hetfield and drummer Lars Ulrich, and has been based in San Francisco for most of its career.";
        HttpContent content = JsonContent.Create(new Artist() { Name = WRONG_NAME, Bio = BIO });
        HttpResponseMessage result = client.PostAsync(_url + ARTISTS, content).Result;
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
        content = JsonContent.Create(new Artist() { Name = RIGHT_NAME, Bio = BIO, Id = 1 });
        result = client.PutAsync(_url + ARTISTS, content).Result;
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

        result = client.GetAsync(_url + ARTISTS).Result;
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

        result = client.GetAsync(_url + string.Format(ARTIST_BY, RIGHT_NAME)).Result;
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

        #region Music
        const string MUSIC_WRONG_NAME = "Master of Pupets";
        const int YEAR_OF_RELEASE = 1986;
        content = JsonContent.Create(new Music() { Name = MUSIC_WRONG_NAME, YearOfRelease = YEAR_OF_RELEASE, Artist = new() { Id = 1 } });
        result = client.PostAsync(_url + MUSICS, content).Result;
        Assert.Multiple(() =>
        {
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(result.Headers.Location?.OriginalString, Is.EqualTo(string.Format(MUSIC_BY, MUSIC_WRONG_NAME)));
        });
        resultContent = result.Content.ReadAsStringAsync().Result;
        Music? music = JsonSerializer.Deserialize<Music>(resultContent, JsonSerializerOptions.Web);
        Assert.That(music, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(music.Name, Is.EqualTo(MUSIC_WRONG_NAME));
            Assert.That(music.YearOfRelease, Is.EqualTo(YEAR_OF_RELEASE));
            Assert.That(music.Id, Is.EqualTo(1));
        });

        artist = music.Artist;
        Assert.That(artist, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(artist.Name, Is.EqualTo(RIGHT_NAME));
            Assert.That(artist.Bio, Is.EqualTo(BIO));
            Assert.That(artist.ProfileImage, Is.EqualTo(Artist.DEFAULT_PROFILE_IMAGE));
            Assert.That(artist.Id, Is.EqualTo(1));
            CollectionAssert.AreEqual(artist.Musics, new Music?[] { null });
        });

        result = client.GetAsync(_url + string.Format(ARTIST_BY, RIGHT_NAME)).Result;
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
        content = JsonContent.Create(new Music() { Name = MUSIC_RIGHT_NAME, YearOfRelease = YEAR_OF_RELEASE, Id = 1, Artist = new() { Id = 1 } });
        result = client.PutAsync(_url + MUSICS, content).Result;
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
            Assert.That(artist.Name, Is.EqualTo(RIGHT_NAME));
            Assert.That(artist.Bio, Is.EqualTo(BIO));
            Assert.That(artist.ProfileImage, Is.EqualTo(Artist.DEFAULT_PROFILE_IMAGE));
            Assert.That(artist.Id, Is.EqualTo(1));
            CollectionAssert.AreEqual(artist.Musics, new Music?[] { null });
        });

        result = client.GetAsync(_url + MUSICS).Result;
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
            Assert.That(artist.Name, Is.EqualTo(RIGHT_NAME));
            Assert.That(artist.Bio, Is.EqualTo(BIO));
            Assert.That(artist.ProfileImage, Is.EqualTo(Artist.DEFAULT_PROFILE_IMAGE));
            Assert.That(artist.Id, Is.EqualTo(1));
            CollectionAssert.AreEqual(artist.Musics, new Music?[] { null });
        });

        result = client.GetAsync(_url + string.Format(MUSIC_BY, MUSIC_RIGHT_NAME)).Result;
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
            Assert.That(artist.Name, Is.EqualTo(RIGHT_NAME));
            Assert.That(artist.Bio, Is.EqualTo(BIO));
            Assert.That(artist.ProfileImage, Is.EqualTo(Artist.DEFAULT_PROFILE_IMAGE));
            Assert.That(artist.Id, Is.EqualTo(1));
            CollectionAssert.AreEqual(artist.Musics, new Music?[] { null });
        });

        result = client.DeleteAsync(_url + string.Format(MUSIC_BY, 1)).Result;
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

        result = client.GetAsync(_url + MUSICS).Result;
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        resultContent = result.Content.ReadAsStringAsync().Result;
        artists = JsonSerializer.Deserialize<Artist[]>(resultContent, JsonSerializerOptions.Web);
        Assert.That(artists, Is.Not.Null);
        Assert.That(artists, Has.Length.EqualTo(0));
        #endregion

        #region Music2
        const string NUMB = "Numb";
        const int NUMB_YOR = 2003;
        content = JsonContent.Create(new Music() { Name = NUMB, YearOfRelease = NUMB_YOR, Artist = new() { Id = 1_000 } });
        result = client.PostAsync(_url + MUSICS, content).Result;
        Assert.Multiple(() =>
        {
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(result.Headers.Location?.OriginalString, Is.EqualTo(string.Format(MUSIC_BY, NUMB)));
        });
        resultContent = result.Content.ReadAsStringAsync().Result;
        music = JsonSerializer.Deserialize<Music>(resultContent, JsonSerializerOptions.Web);
        Assert.That(music, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(music.Name, Is.EqualTo(NUMB));
            Assert.That(music.YearOfRelease, Is.EqualTo(NUMB_YOR));
            Assert.That(music.Id, Is.EqualTo(2));
        });

        artist = music.Artist;
        Assert.That(artist, Is.Null);

        result = client.DeleteAsync(_url + string.Format(MUSIC_BY, 2)).Result;
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

        const string IN_THE_END = "In the end";
        const int IN_THE_END_YOR = 2001;
        content = JsonContent.Create(new Music() { Name = IN_THE_END, YearOfRelease = IN_THE_END_YOR });
        result = client.PostAsync(_url + MUSICS, content).Result;
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
            Assert.That(music.Id, Is.EqualTo(3));
        });

        artist = music.Artist;
        Assert.That(artist, Is.Null);

        result = client.DeleteAsync(_url + string.Format(MUSIC_BY, 3)).Result;
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        #endregion

        result = client.DeleteAsync(_url + string.Format(ARTIST_BY, 1)).Result;
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

        result = client.GetAsync(_url + ARTISTS).Result;
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        resultContent = result.Content.ReadAsStringAsync().Result;
        artists = JsonSerializer.Deserialize<Artist[]>(resultContent, JsonSerializerOptions.Web);
        Assert.That(artists, Is.Not.Null);
        Assert.That(artists, Has.Length.EqualTo(0));

    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _webApp.Dispose();

        SqlConnection.ClearAllPools();

        const string DROP_SSTEST = $"DROP DATABASE {SSTEST_NAME}";
        using SqlCommand dropCommand = new(DROP_SSTEST, _serverConnection);

        try
        {
            dropCommand.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            TestContext.Out.WriteLine(e.ToString());
        }
        _serverConnection.Dispose();
    }
}