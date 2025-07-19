using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using screensound.api.endpoints;
using screensound.core.data;
using screensound.core.data.dal;
using screensound.core.models;
using System.Text.Json.Serialization;
using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace screensound.api;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<JsonOptions>(ConfigureJson);

        builder.Services.AddDbContext<ScreenSoundContext>();
        builder.Services.AddTransient<DAL<Artist>>();
        builder.Services.AddTransient<DAL<Music>>();

        WebApplication app = builder.Build();
        app.AddArtistsEndpoints();
        app.AddMusicsEndpoints();
        app.Run();
    }

    private static void ConfigureJson(JsonOptions options)
    {
        options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.SerializerOptions.WriteIndented = true;
    }
}
