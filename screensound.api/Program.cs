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
        builder.Services.AddTransient<DAL<Music>>();

        WebApplication app = builder.Build();

        app.MapGet("/artists", GetArtists);
        app.MapGet(string.Format(GET_ARTIST_ROUTE, "{name}"), GetArtist);
        app.MapPost("/artists", PostArtist);
        app.MapDelete("/artists/{id}", RemoveArtist);
        app.MapPut("/artists", UpdateArtist);

        app.MapGet("/musics", GetMusics);
        app.MapGet(string.Format(GET_MUSICS_BY_NAME_ROUTE, "{name}"), GetMusicsByName);
        app.MapPost("/musics", PostMusic);
        app.MapDelete("/musics/{id}", RemoveMusic);
        app.MapPut("/musics", UpdateMusic);


        app.Run();
    }

    private static void ConfigureJson(JsonOptions options)
    {
        options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.SerializerOptions.WriteIndented = true;
    }

    #region Artists
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
    #endregion

    #region Artists
    private static async Task<IResult> GetMusics([FromServices] DAL<Music> dal)
    {
        List<Music> result = await dal.GetListAsync();
        return Results.Ok(result);
    }

    private const string GET_MUSICS_BY_NAME_ROUTE = "/musics/{0}";
    private static async Task<IResult> GetMusicsByName([FromServices] DAL<Music> dal, string name)
    {
        List<Music> data = await dal.WhereAsync(Predicate);
        bool Predicate(Music music)
        {
            return name.Equals(music.Name, StringComparison.CurrentCultureIgnoreCase);
        }

        if (data is null)
            return Results.NotFound();
        else
            return Results.Ok(data);
    }
    private static async Task<IResult> PostMusic(
        [FromServices] DAL<Music> mdal,
        [FromServices] DAL<Artist> adal,
        [FromBody] Music music)
    {
        Artist? artist = music.Artist;
        music.Artist = null;

        EntityEntry<Music> entity = await mdal.AddAsync(music);

        if (artist != null && artist.Id > 0)
        {
            artist = await adal.FirstAsync(a => a.Id == artist.Id);
            if (artist != null)
            {
                artist.AddMusic(entity.Entity);
                await adal.UpdateAsync(artist);
            }
        }

        return Results.Created(string.Format(GET_ARTIST_ROUTE, music.Name), entity.Entity);
    }

    private static async Task<IResult> RemoveMusic([FromServices] DAL<Music> dal, int id)
    {
        Music? artist = await dal.FirstAsync(dal => dal.Id == id);
        if (artist is null)
            return Results.NotFound();

        await dal.RemoveAsync(artist);
        return Results.NoContent();
    }

    private static async Task<IResult> UpdateMusic([FromServices] DAL<Music> dal, [FromBody] Music music)
    {
        Music? artistOnDb = await dal.FirstAsync(m => m.Id == music.Id);

        if (artistOnDb is null)
            return Results.NotFound();

        artistOnDb.Name = music.Name;
        artistOnDb.YearOfRelease = music.YearOfRelease;

        EntityEntry<Music> result = await dal.UpdateAsync(artistOnDb);
        return Results.Ok(result.Entity);
    }
    #endregion
}
