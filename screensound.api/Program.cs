using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
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

        WebApplication app = builder.Build();

        app.MapGet("/artists", GetArtists);
        app.MapGet("/artists/{name}", GetArtistsByName);
        app.MapPost("/artists", PostArtist);

        app.Run();
    }

    private static void ConfigureJson(JsonOptions options)
    {
        options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.SerializerOptions.WriteIndented = true;
    }

    private static IResult GetArtists()
    {
        ScreenSoundContext context = new();
        DAL<Artist> dal = new(context);
        return Results.Ok(dal.GetList());
    }

    private static IResult GetArtistsByName(string name)
    {
        ScreenSoundContext context = new();
        DAL<Artist> dal = new(context);
        Artist? data = dal.First(a => name.Equals(a.Name, System.StringComparison.CurrentCultureIgnoreCase));
        if (data is null)
            return Results.NotFound();
        else
            return Results.Ok(data);
    }

    private static IResult PostArtist([FromBody] Artist artist)
    {
        ScreenSoundContext context = new();
        DAL<Artist> dal = new(context);
        EntityEntry<Artist> entity = dal.Add(artist);
        return Results.Ok(entity.Entity);
    }

}
