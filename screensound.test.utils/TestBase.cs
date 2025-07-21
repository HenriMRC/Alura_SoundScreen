using Microsoft.Data.SqlClient;
using NUnit.Framework;

namespace screensound.test.utils;

public abstract class TestBase
{
    protected const string SERVER_CONNECTION_STRING =
        "Data Source=(localdb)\\MSSQLLocalDB;" +
        "Initial Catalog=master;" +
        "Integrated Security=True;" +
        "Connect Timeout=30;" +
        "Encrypt=False;" +
        "Trust Server Certificate=False;" +
        "Application Intent=ReadWrite;" +
        "Multi Subnet Failover=False";

    protected const string DB_CONNECTION_STRING =
        "Data Source=(localdb)\\MSSQLLocalDB;" +
        $"Initial Catalog={DB_NAME};" +
        "Integrated Security=True;" +
        "Connect Timeout=30;" +
        "Encrypt=False;" +
        "Trust Server Certificate=False;" +
        "Application Intent=ReadWrite;" +
        "Multi Subnet Failover=False";

    protected const string DB_NAME = "ScreenSoundTest";

    protected SqlConnection _serverConnection = null!;

    [OneTimeSetUp]
    public virtual void OneTimeSetUp()
    {
        _serverConnection = new(SERVER_CONNECTION_STRING);
        _serverConnection.Open();

        // ===== Create DB =====
        const string CREATE_DB = $"CREATE DATABASE {DB_NAME}";
        using SqlCommand createDbCommand = new(CREATE_DB, _serverConnection);
        createDbCommand.ExecuteNonQuery();

        using SqlConnection dbConnection = new(DB_CONNECTION_STRING);
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
    }

    [OneTimeTearDown]
    public virtual void OneTimeTearDown()
    {
        SqlConnection.ClearAllPools();

        const string DROP_DB = $"DROP DATABASE {DB_NAME}";
        using SqlCommand dropCommand = new(DROP_DB, _serverConnection);

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
