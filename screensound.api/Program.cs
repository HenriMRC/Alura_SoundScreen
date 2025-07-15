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

        builder.Services.AddDbContext<ScreenSoundContext>();
        builder.Services.AddTransient<DAL<Artist>>();

        WebApplication app = builder.Build();

        app.MapGet("/artists", GetArtists);
        app.MapGet(string.Format(GET_ARTIST_BY_NAME_ROUTE, "{name}"), GetArtistsByName);
        app.MapPost("/artists", PostArtist);
        app.MapDelete("/artists/{id}", RemoveArtist);
        app.MapPut("/artists", UpdateArtist);

        app.Run();
    }

    private static void ConfigureJson(JsonOptions options)
    {
        options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.SerializerOptions.WriteIndented = true;
    }

    private static IResult GetArtists([FromServices] DAL<Artist> dal)
    {
        return Results.Ok(dal.GetList());
    }

    private const string GET_ARTIST_BY_NAME_ROUTE = "/artists/{0}";
    private static IResult GetArtistsByName([FromServices] DAL<Artist> dal, string name)
    {
        Artist? data = dal.First(a => name.Equals(a.Name, System.StringComparison.CurrentCultureIgnoreCase));
        if (data is null)
            return Results.NotFound();
        else
            return Results.Ok(data);
    }

    private static IResult PostArtist([FromServices] DAL<Artist> dal, [FromBody] Artist artist)
    {
        EntityEntry<Artist> entity = dal.Add(artist);
        return Results.Created(string.Format(GET_ARTIST_BY_NAME_ROUTE, artist.Name), entity.Entity);
    }

    private static IResult RemoveArtist([FromServices] DAL<Artist> dal, int id)
    {
        Artist? artist = dal.First(dal => dal.Id == id);
        if (artist is null)
            return Results.NotFound();

        dal.Remove(artist);
        return Results.NoContent();
    }
    private static IResult UpdateArtist([FromServices] DAL<Artist> dal, [FromBody] Artist artist)
    {
        Artist? artistOnDb = dal.First(a => a.Id == artist.Id);

        if (artistOnDb is null)
            return Results.NotFound();

        artistOnDb.Name = artist.Name;
        artistOnDb.Bio = artist.Bio;
        artistOnDb.ProfileImage = artist.ProfileImage;

        EntityEntry<Artist> result = dal.Update(artistOnDb);
        return Results.Ok(result.Entity);
    }
}
