using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using screensound.database;
using screensound.models;
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

            const string CREATE_SSTEST = $"CREATE DATABASE {SSTEST_NAME}";
            using SqlCommand createSsTestCommand = new(CREATE_SSTEST, _serverConnection);
            createSsTestCommand.ExecuteNonQuery();

            using SqlConnection dbConnection = new(SSTEST_CONNECTION_STRING);
            dbConnection.Open();

            const string CREATE_MUSIC_TABLE =
                """
                CREATE TABLE Musics(
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        Name NVARCHAR(255) NOT NULL);
                """;
            using SqlCommand createMusicDbCommand = new(CREATE_MUSIC_TABLE, dbConnection);
            createMusicDbCommand.ExecuteNonQuery();
        }

        [Test]
        public void Test()
        {
            TestContext.Progress.WriteLine(nameof(Tests));

            using ScreenSoundContext context = new(SSTEST_CONNECTION_STRING);
            MusicDAL dal = new(context);

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

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            const string DROP_SSTEST = $"DROP DATABASE {SSTEST_NAME}";
            using SqlCommand dropCommand = new(DROP_SSTEST, _serverConnection);
            dropCommand.ExecuteNonQuery();
            _serverConnection.Dispose();
        }
    }
}