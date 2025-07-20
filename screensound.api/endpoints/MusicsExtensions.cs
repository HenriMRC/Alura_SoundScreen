using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using screensound.core.data.dal;
using screensound.core.models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using static screensound.api.endpoints.Route;

namespace screensound.api.endpoints;

public static class MusicsExtensions
{
    public static void AddMusicsEndpoints(this WebApplication app)
    {
        app.MapGet(MUSICS, GetMusics);
        static async Task<IResult> GetMusics([FromServices] DAL<Music> dal)
        {
            List<Music> result = await dal.GetListAsync();
            return Results.Ok(result);
        }

        app.MapGet(string.Format(MUSIC_BY, "{name}"), GetMusicsByName);
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

        app.MapPost(MUSICS, PostMusic);
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

            //TODO: Change to return some ReturnData<T> that contains warnings 
            //in the case that the artist is no found.
            return Results.Created(string.Format(MUSIC_BY, music.Name), entity.Entity);
        }

        app.MapDelete(string.Format(MUSIC_BY, "{id}"), RemoveMusic);
        static async Task<IResult> RemoveMusic([FromServices] DAL<Music> dal, int id)
        {
            Music? artist = await dal.FirstAsync(dal => dal.Id == id);
            if (artist is null)
                return Results.NotFound();

            await dal.RemoveAsync(artist);
            return Results.NoContent();
        }

        app.MapPut(MUSICS, UpdateMusic);
        static async Task<IResult> UpdateMusic([FromServices] DAL<Music> dal, [FromBody] Music music)
        {
            Music? musicOnDb = await dal.FirstAsync(m => m.Id == music.Id);

            if (musicOnDb is null)
                return Results.NotFound();

            musicOnDb.Name = music.Name;
            musicOnDb.YearOfRelease = music.YearOfRelease;
            //TODO: change artist

            EntityEntry<Music> result = await dal.UpdateAsync(musicOnDb);
            return Results.Ok(result.Entity);
        }
    }
}
