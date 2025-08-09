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

public static class GenreExtensions
{
    public static void AddGenresEndpoints(this WebApplication app)
    {
        app.MapGet(GENRES, GetGenres);
        static async Task<IResult> GetGenres([FromServices] DAL<Genre> dal)
        {
            List<Genre> result = await dal.GetListAsync();

            GenreResponse[] response = [.. result.Select(m => (GenreResponse)m)];
            return Results.Ok(response);
        }

        app.MapGet(string.Format(GENRES_BY, "{name}"), GetMusicsByName);
        static async Task<IResult> GetMusicsByName([FromServices] DAL<Genre> dal, string name)
        {
            List<Genre> result = await dal.WhereAsync(Predicate);
            bool Predicate(Genre genre)
            {
                return name.Equals(genre.Name, StringComparison.CurrentCultureIgnoreCase);
            }
            GenreResponse[] response = [.. result.Select(m => (GenreResponse)m)];
            return Results.Ok(response);
        }

        app.MapPost(GENRES, PostMusic);
        static async Task<IResult> PostMusic([FromServices] DAL<Genre> dal, [FromBody] GenreRequest genre)
        {
            EntityEntry<Genre> result = await dal.AddAsync(genre);

            GenreResponse response = result.Entity;
            return Results.Created(string.Format(GENRES_BY, genre.Name), response);
        }

        app.MapDelete(string.Format(GENRES_BY, "{id}"), RemoveMusic);
        static async Task<IResult> RemoveMusic([FromServices] DAL<Genre> dal, int id)
        {
            Genre? result = await dal.FirstAsync(g => g.Id == id);
            if (result is null)
                return Results.NotFound();

            await dal.RemoveAsync(result);
            return Results.NoContent();
        }

        app.MapPut(GENRES, UpdateMusic);
        static async Task<IResult> UpdateMusic([FromServices] DAL<Genre> dal, [FromBody] UpdateGenreRequest genre)
        {
            Genre? genreOnDb = await dal.FirstAsync(g => g.Id == genre.Id);
            if (genreOnDb is null)
                return Results.NotFound();

            if (!string.IsNullOrWhiteSpace(genre.Name))
                genreOnDb.Name = genre.Name;
            if (!string.IsNullOrWhiteSpace(genre.Description))
                genreOnDb.Description = genre.Description;

            EntityEntry<Genre> result = await dal.UpdateAsync(genreOnDb);

            GenreResponse response = result.Entity;
            return Results.Ok(response);
        }
    }

}
