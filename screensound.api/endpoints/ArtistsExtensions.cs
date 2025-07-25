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

public static class ArtistsExtensions
{
    public static void AddArtistsEndpoints(this WebApplication app)
    {
        app.MapGet(ARTISTS, GetArtists);
        static async Task<IResult> GetArtists([FromServices] DAL<Artist> dal)
        {
            List<Artist> result = await dal.GetListAsync();
            return Results.Ok(result);
        }

        app.MapGet(string.Format(ARTIST_BY, "{name}"), GetArtist);
        static async Task<IResult> GetArtist([FromServices] DAL<Artist> dal, string name)
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

        app.MapPost(ARTISTS, PostArtist);
        static async Task<IResult> PostArtist([FromServices] DAL<Artist> dal, [FromBody] ArtistRequest artist)
        {
            EntityEntry<Artist> entity = await dal.AddAsync(artist);
            return Results.Created(string.Format(ARTIST_BY, artist.Name), entity.Entity);
        }

        app.MapDelete(string.Format(ARTIST_BY, "{id}"), RemoveArtist);
        static async Task<IResult> RemoveArtist([FromServices] DAL<Artist> dal, int id)
        {
            Artist? artist = await dal.FirstAsync(dal => dal.Id == id);
            if (artist is null)
                return Results.NotFound();

            await dal.RemoveAsync(artist);
            return Results.NoContent();
        }

        app.MapPut(ARTISTS, UpdateArtist);
        static async Task<IResult> UpdateArtist([FromServices] DAL<Artist> dal, [FromBody] UpdateArtistRequest artist)
        {
            Artist? artistOnDb = await dal.FirstAsync(a => a.Id == artist.Id);

            if (artistOnDb is null)
                return Results.NotFound();

            if (!string.IsNullOrWhiteSpace(artist.Name))
                artistOnDb.Name = artist.Name;
            if (!string.IsNullOrWhiteSpace(artist.Bio))
                artistOnDb.Bio = artist.Bio;
            if (!string.IsNullOrWhiteSpace(artist.ProfileImage))
                artistOnDb.ProfileImage = artist.ProfileImage;

            EntityEntry<Artist> result = await dal.UpdateAsync(artistOnDb);
            return Results.Ok(result.Entity);
        }
    }
}
