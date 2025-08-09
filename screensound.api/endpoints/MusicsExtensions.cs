using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using screensound.api.requests;
using screensound.api.responses;
using screensound.core.data.dal;
using screensound.core.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using static screensound.api.endpoints.Routes;

namespace screensound.api.endpoints;

public static class MusicsExtensions
{
    public static void AddMusicsEndpoints(this WebApplication app)
    {
        app.MapGet(MUSICS, GetMusics);
        static async Task<IResult> GetMusics([FromServices] DAL<Music> dal)
        {
            List<Music> result = await dal.GetListAsync();

            MusicResponse[] response = [.. result.Select(m => (MusicResponse)m)];
            return Results.Ok(response);
        }

        app.MapGet(string.Format(MUSICS_BY, "{name}"), GetMusicsByName);
        static async Task<IResult> GetMusicsByName([FromServices] DAL<Music> dal, string name)
        {
            List<Music> result = await dal.WhereAsync(Predicate);
            bool Predicate(Music music)
            {
                return name.Equals(music.Name, StringComparison.CurrentCultureIgnoreCase);
            }

            if (result is null)
                return Results.NotFound();
            else
            {
                MusicResponse[] response = [.. result.Select(m => (MusicResponse)m)];
                return Results.Ok(response);
            }
        }

        app.MapPost(MUSICS, PostMusic);
        static async Task<IResult> PostMusic([FromServices] DAL<Music> mdal, [FromServices] DAL<Artist> adal, [FromBody] MusicRequest music)
        {
            Artist? artist = await adal.FirstAsync(a => a.Id == music.ArtistId);
            if (artist == null)
                return Results.NotFound("Artist not found");

            (string name, _, int? yearOfRelease, _) = music;
            Music musicForDb = new(name)
            {
                Artist = artist,
                YearOfRelease = yearOfRelease,
            };
            EntityEntry<Music> result = await mdal.AddAsync(musicForDb);

            MusicResponse response = result.Entity;
            return Results.Created(string.Format(MUSICS_BY, music.Name), response);
        }

        app.MapDelete(string.Format(MUSICS_BY, "{id}"), RemoveMusic);
        static async Task<IResult> RemoveMusic([FromServices] DAL<Music> dal, int id)
        {
            Music? music = await dal.FirstAsync(dal => dal.Id == id);
            if (music is null)
                return Results.NotFound();

            await dal.RemoveAsync(music);
            return Results.NoContent();
        }

        app.MapPut(MUSICS, UpdateMusic);
        static async Task<IResult> UpdateMusic([FromServices] DAL<Music> mdal, [FromServices] DAL<Artist> adal, [FromBody] UpdateMusicRequest music)
        {
            Music? musicOnDb = await mdal.FirstAsync(m => m.Id == music.Id);
            if (musicOnDb is null)
                return Results.NotFound();

            if (!string.IsNullOrWhiteSpace(music.Name))
                musicOnDb.Name = music.Name;
            if (music.YearOfRelease.HasValue)
                musicOnDb.YearOfRelease = music.YearOfRelease;
            if (music.ArtistId.HasValue)
            {
                Artist? artist = await adal.FirstAsync(a => a.Id == music.ArtistId);
                if (artist == null)
                    return Results.NotFound("Artist not fount");
                musicOnDb.Artist = artist;
            }

            EntityEntry<Music> result = await mdal.UpdateAsync(musicOnDb);

            MusicResponse response = result.Entity;
            return Results.Ok(response);
        }
    }
}
