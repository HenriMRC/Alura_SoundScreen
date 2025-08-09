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

public static class ArtistsExtensions
{
    public static void AddArtistsEndpoints(this WebApplication app)
    {
        app.MapGet(ARTISTS, GetArtists);
        static async Task<IResult> GetArtists([FromServices] DAL<Artist> dal)
        {
            List<Artist> result = await dal.GetListAsync();
            ArtistResponse[] response = [.. result.Select(a => (ArtistResponse)a)];
            return Results.Ok(response);
        }

        app.MapGet(string.Format(ARTISTS_BY, "{name}"), GetArtist);
        static async Task<IResult> GetArtist([FromServices] DAL<Artist> dal, string name)
        {
            List<Artist> result = await dal.WhereAsync(Predicate);
            bool Predicate(Artist artist)
            {
                return name.Equals(artist.Name, StringComparison.CurrentCultureIgnoreCase);
            }
            ArtistResponse[] response = [.. result.Select(m => (ArtistResponse)m)];
            return Results.Ok(response);
        }

        app.MapPost(ARTISTS, PostArtist);
        static async Task<IResult> PostArtist([FromServices] DAL<Artist> dal, [FromBody] ArtistRequest artist)
        {
            EntityEntry<Artist> result = await dal.AddAsync(artist);
            ArtistResponse response = result.Entity;
            return Results.Created(string.Format(ARTISTS_BY, artist.Name), response);
        }

        app.MapDelete(string.Format(ARTISTS_BY, "{id}"), RemoveArtist);
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
            ArtistResponse response = result.Entity;
            return Results.Ok(response);
        }
    }
}
