using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using screensound.api.endpoints;
using screensound.core.data;
using screensound.core.data.dal;
using screensound.core.models;
using System;
using System.Text.Json.Serialization;
using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace screensound.api;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplication app = GetApp(args, null);
        app.Run();
    }

    public static WebApplication GetApp(string[] args, Action<DbContextOptionsBuilder>? dbContextAction)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<JsonOptions>(ConfigureJson);

        builder.Services.AddDbContext<ScreenSoundContext>(dbContextAction);
        builder.Services.AddTransient<DAL<Artist>>();
        builder.Services.AddTransient<DAL<Music>>();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        WebApplication app = builder.Build();
        app.AddArtistsEndpoints();
        app.AddMusicsEndpoints();

        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }

    private static void ConfigureJson(JsonOptions options)
    {
        options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.SerializerOptions.WriteIndented = true;
    }
}
