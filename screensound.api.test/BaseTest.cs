using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using screensound.core.data;
using screensound.test.utils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace screensound.api.test;

public abstract class BaseTest
{
    protected abstract string DbName { get; }
    protected string Uri => _uri;
    protected ScreenSoundContext Context => _context;

    private IDisposable _webApp;
    private IDisposable _scope;
    private ScreenSoundContext _context;
    private string _uri;

    [OneTimeSetUp]
    public virtual void OneTimeSetUp()
    {
        WebApplication webApp = Program.GetApp([], DbContextAction);
        static void DbContextAction(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(TestUtils.GetSqlConnectionString("ScreenSoundTest"));
        }

        IServiceScope scope = webApp.Services.CreateScope();

        ScreenSoundContext? context = scope.ServiceProvider.GetService<ScreenSoundContext>();
        if (context == null)
            throw new NullReferenceException($"{nameof(context)} is null.");

        context.Database.EnsureCreated();

        _webApp = webApp;
        _scope = scope;
        _context = context;

        Task startupTask = webApp.StartAsync();
        startupTask.Wait();
        _uri = webApp.Urls.First();
    }

    [OneTimeTearDown]
    public virtual void OneTimeTearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
        _scope.Dispose();
        _webApp.Dispose();
    }
}
