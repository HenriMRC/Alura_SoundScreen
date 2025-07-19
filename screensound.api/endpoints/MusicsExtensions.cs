using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using screensound.core.data.dal;
using screensound.core.models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace screensound.api.endpoints;

public static class MusicsExtensions
{
    public static void AddMusicsEndpoints(this WebApplication app)
    {
        app.MapGet("/musics", GetMusics);
        static async Task<IResult> GetMusics([FromServices] DAL<Music> dal)
        {
            List<Music> result = await dal.GetListAsync();
            return Results.Ok(result);
        }

        const string GET_MUSICS_BY_NAME_ROUTE = "/musics/{0}";
        app.MapGet(string.Format(GET_MUSICS_BY_NAME_ROUTE, "{name}"), GetMusicsByName);
        static async Task<IResult> GetMusicsByName([FromServices] DAL<Music> dal, string name)
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

        app.MapPost("/musics", PostMusic);
        static async Task<IResult> PostMusic([FromServices] DAL<Music> mdal, [FromServices] DAL<Artist> adal, [FromBody] Music music)
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

            return Results.Created(string.Format(GET_MUSICS_BY_NAME_ROUTE, music.Name), entity.Entity);
        }

        app.MapDelete("/musics/{id}", RemoveMusic);
        static async Task<IResult> RemoveMusic([FromServices] DAL<Music> dal, int id)
        {
            Music? artist = await dal.FirstAsync(dal => dal.Id == id);
            if (artist is null)
                return Results.NotFound();

            await dal.RemoveAsync(artist);
            return Results.NoContent();
        }

        app.MapPut("/musics", UpdateMusic);
        static async Task<IResult> UpdateMusic([FromServices] DAL<Music> dal, [FromBody] Music music)
        {
            Music? artistOnDb = await dal.FirstAsync(m => m.Id == music.Id);

            if (artistOnDb is null)
                return Results.NotFound();

            artistOnDb.Name = music.Name;
            artistOnDb.YearOfRelease = music.YearOfRelease;

            EntityEntry<Music> result = await dal.UpdateAsync(artistOnDb);
            return Results.Ok(result.Entity);
        }
    }
}
