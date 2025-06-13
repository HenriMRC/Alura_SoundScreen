using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using screensound.database;
using screensound.database.dal;
using screensound.models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace screensound.test
{
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
        }

        [Test]
        public void TestMusics()
        {
            TestContext.Progress.WriteLine(nameof(TestMusics));

            using ScreenSoundContext context = new(SSTEST_CONNECTION_STRING);

            DAL<Music> dal = new(context);

            Task<List<Music>> listTask = dal.GetListAsync();
            do
            {
                TestContext.Progress.WriteLine("Getting music list 1");
            }
            while (!listTask.IsCompleted);

            if (listTask.Exception != null)
                throw listTask.Exception;

            Assert.That(listTask.IsCompletedSuccessfully, Is.True);
            Assert.That(listTask.Result, Has.Count.EqualTo(0));

            const string NAME = "TestMusic";
            Task<EntityEntry<Music>> task = dal.AddAsync(new(NAME));
            do
            {
                TestContext.Progress.WriteLine("Adding music");
            }
            while (!task.IsCompleted);

            Assert.That(task.IsCompletedSuccessfully, Is.True);
            Assert.That(task.Result.IsKeySet, Is.True);
            Assert.That(task.Result.Entity.Name, Is.EqualTo(NAME));

            listTask = dal.GetListAsync();
            do
            {
                TestContext.Progress.WriteLine("Getting music list 2");
            }
            while (!listTask.IsCompleted);

            if (listTask.Exception != null)
                throw listTask.Exception;

            Assert.That(listTask.IsCompletedSuccessfully, Is.True);
            Assert.That(listTask.Result, Has.Count.EqualTo(1));

            const string NEW_NAME = "TestMusic2";
            TestContext.Progress.WriteLine("Updating music");
            task.Result.Entity.Name = NEW_NAME;
            EntityEntry<Music> result = dal.Update(task.Result.Entity);
            Assert.That(task.Result.IsKeySet, Is.True);
            Assert.That(task.Result.Entity.Name, Is.EqualTo(NEW_NAME));

            TestContext.Progress.WriteLine("Removing music");
            result = dal.Remove(task.Result.Entity);
            Assert.That(task.Result.IsKeySet, Is.True);
            Assert.That(task.Result.Entity.Name, Is.EqualTo(NEW_NAME));

            listTask = dal.GetListAsync();
            do
            {
                TestContext.Progress.WriteLine("Getting music list 3");
            }
            while (!listTask.IsCompleted);

            if (listTask.Exception != null)
                throw listTask.Exception;

            Assert.That(listTask.IsCompletedSuccessfully, Is.True);
            Assert.That(listTask.Result, Has.Count.EqualTo(0));
        }

        [Test]
        public void TestArtists()
        {
            TestContext.Progress.WriteLine(nameof(TestArtists));

            using ScreenSoundContext context = new(SSTEST_CONNECTION_STRING);
            DAL<Artist> dal = new(context);

            // =======================
            // Get artist list
            Task<List<Artist>> listTask = dal.GetListAsync();
            do
            {
                TestContext.Progress.WriteLine("Getting artist list 1");
            }
            while (!listTask.IsCompleted);

            if (listTask.Exception != null)
                throw listTask.Exception;

            Assert.That(listTask.IsCompletedSuccessfully, Is.True);
            Assert.That(listTask.Result, Has.Count.EqualTo(0));

            // =======================
            // Add artist
            const string NAME = "TestArtist";
            Task<EntityEntry<Artist>> task = dal.AddAsync(new(NAME, string.Empty));
            do
            {
                TestContext.Progress.WriteLine("Adding artist 1");
            }
            while (!task.IsCompleted);

            Assert.That(task.IsCompletedSuccessfully, Is.True);
            Assert.That(task.Result.IsKeySet, Is.True);
            Assert.That(task.Result.Entity.Name, Is.EqualTo(NAME));

            // =======================
            // Get artist list
            listTask = dal.GetListAsync();
            do
            {
                TestContext.Progress.WriteLine("Getting artist list 2");
            }
            while (!listTask.IsCompleted);

            if (listTask.Exception != null)
                throw listTask.Exception;

            Assert.That(listTask.IsCompletedSuccessfully, Is.True);
            Assert.That(listTask.Result, Has.Count.EqualTo(1));

            // =======================
            // Add artist
            task = dal.AddAsync(new(NAME, string.Empty));
            do
            {
                TestContext.Progress.WriteLine("Adding artist 2");
            }
            while (!task.IsCompleted);

            Assert.That(task.IsCompletedSuccessfully, Is.True);
            Assert.That(task.Result.IsKeySet, Is.True);
            Assert.That(task.Result.Entity.Name, Is.EqualTo(NAME));

            // =======================
            // Get first artist
            Artist? result = dal.FirstAsync(a => a.Name.Equals(NAME)).Result;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(NAME));
            Assert.That(result.Id, Is.EqualTo(1));

            // =======================
            // Get artist list
            listTask = dal.GetListAsync();
            do
            {
                TestContext.Progress.WriteLine("Getting artist list 2");
            }
            while (!listTask.IsCompleted);

            if (listTask.Exception != null)
                throw listTask.Exception;

            Assert.That(listTask.IsCompletedSuccessfully, Is.True);
            Assert.That(listTask.Result, Has.Count.EqualTo(2));

            // =======================
            // Update artist
            const string NEW_NAME = "TestArtist2";
            TestContext.Progress.WriteLine("Updating artist");
            task.Result.Entity.Name = NEW_NAME;
            EntityEntry<Artist> entity = dal.Update(task.Result.Entity);
            Assert.That(task.Result.IsKeySet, Is.True);
            Assert.That(task.Result.Entity.Name, Is.EqualTo(NEW_NAME));

            // =======================
            // Remove artist
            TestContext.Progress.WriteLine("Removing artist");
            entity = dal.Remove(task.Result.Entity);
            Assert.That(task.Result.IsKeySet, Is.True);
            Assert.That(task.Result.Entity.Name, Is.EqualTo(NEW_NAME));

            // =======================
            // Get artist list
            listTask = dal.GetListAsync();
            do
            {
                TestContext.Progress.WriteLine("Getting artist list 3");
            }
            while (!listTask.IsCompleted);

            if (listTask.Exception != null)
                throw listTask.Exception;

            Assert.That(listTask.IsCompletedSuccessfully, Is.True);
            Assert.That(listTask.Result, Has.Count.EqualTo(1));
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
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
}