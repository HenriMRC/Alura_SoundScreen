using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using screensound.core.data;
using screensound.core.data.dal;
using screensound.core.models;
using screensound.test.utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace screensound.core.test;

public class Tests : TestBase
{
    [Test]
    public void TestMusics()
    {
        TestContext.Progress.WriteLine(nameof(TestMusics));

        DbContextOptionsBuilder builder = new();
        builder.UseSqlServer(DB_CONNECTION_STRING);
        using ScreenSoundContext context = new(builder.Options);

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

        DbContextOptionsBuilder builder = new();
        builder.UseSqlServer(DB_CONNECTION_STRING);
        using ScreenSoundContext context = new(builder.Options);
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
}