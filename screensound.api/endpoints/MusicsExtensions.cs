using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using screensound.api.requests;
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
        static async Task<IResult> PostMusic([FromServices] DAL<Music> mdal, [FromServices] DAL<Artist> adal, [FromBody] MusicRequest music)
        {
            MusicAdder? adder = await MusicAdder.Get(adal, music.ArtistId);
            if (adder == null)
                return Results.NotFound($"Artist {music.ArtistId} not found");

            EntityEntry<Music> entity = await mdal.AddAsync(music);

            await adder.DoAdd(entity.Entity);

            return Results.Created(string.Format(MUSIC_BY, music.Name), entity.Entity);
        }

        app.MapDelete(string.Format(MUSIC_BY, "{id}"), RemoveMusic);
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

            MusicAdder? adder;
            if (music.ArtistId.HasValue)
            {
                adder = await MusicAdder.Get(adal, music.ArtistId.Value);
                if (adder == null)
                    return Results.NotFound($"Artist {music.ArtistId.Value} not found");
            }
            else
                adder = null;


            if (!string.IsNullOrWhiteSpace(music.Name))
                musicOnDb.Name = music.Name;
            if (music.YearOfRelease.HasValue)
                musicOnDb.YearOfRelease = music.YearOfRelease;

            EntityEntry<Music> result = await mdal.UpdateAsync(musicOnDb);

            adder?.DoAdd(result.Entity);

            return Results.Ok(result.Entity);
        }
    }

    private class MusicAdder
    {
        private readonly Artist _artist;
        private readonly DAL<Artist> _dal;

        private MusicAdder(DAL<Artist> dal, Artist artist)
        {
            _artist = artist;
            _dal = dal;
        }

        internal static async Task<MusicAdder?> Get(DAL<Artist> dal, int artistId)
        {
            Artist? artist = await dal.FirstAsync(a => a.Id == artistId);
            if (artist == null)
                return null;
            else
                return new(dal, artist);
        }

        internal async Task DoAdd(Music music)
        {
            _artist.AddMusic(music);
            await _dal.UpdateAsync(_artist);
        }
    }
}
