using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using screensound.core.data;
using screensound.core.data.dal;
using screensound.core.models;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
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
        app.MapGet(string.Format(GET_ARTIST_ROUTE, "{name}"), GetArtist);
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

    private static async Task<IResult> GetArtists([FromServices] DAL<Artist> dal)
    {
        List<Artist> result = await dal.GetListAsync();
        return Results.Ok(result);
    }

    private const string GET_ARTIST_ROUTE = "/artists/{0}";
    private static async Task<IResult> GetArtist([FromServices] DAL<Artist> dal, string name)
    {
        Artist? data = await dal.FirstAsync(Predicate);
        bool Predicate(Artist artist)
        {
            return name.Equals(artist.Name, StringComparison.CurrentCultureIgnoreCase);
        }

        if (data is null)
            return Results.NotFound();
        else
            return Results.Ok(data);
    }

    private static async Task<IResult> PostArtist([FromServices] DAL<Artist> dal, [FromBody] Artist artist)
    {
        EntityEntry<Artist> entity = await dal.AddAsync(artist);
        return Results.Created(string.Format(GET_ARTIST_ROUTE, artist.Name), entity.Entity);
    }

    private static async Task<IResult> RemoveArtist([FromServices] DAL<Artist> dal, int id)
    {
        Artist? artist = await dal.FirstAsync(dal => dal.Id == id);
        if (artist is null)
            return Results.NotFound();

        await dal.RemoveAsync(artist);
        return Results.NoContent();
    }

    private static async Task<IResult> UpdateArtist([FromServices] DAL<Artist> dal, [FromBody] Artist artist)
    {
        Artist? artistOnDb = await dal.FirstAsync(a => a.Id == artist.Id);

        if (artistOnDb is null)
            return Results.NotFound();

        artistOnDb.Name = artist.Name;
        artistOnDb.Bio = artist.Bio;
        artistOnDb.ProfileImage = artist.ProfileImage;

        EntityEntry<Artist> result = await dal.UpdateAsync(artistOnDb);
        return Results.Ok(result.Entity);
    }
}
