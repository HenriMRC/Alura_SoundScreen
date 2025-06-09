using Microsoft.EntityFrameworkCore;
using screensound.database.dal;
using screensound.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace screensound.database
{
    internal class ArtistDAL : DAL<Artist, ScreenSoundContext>
    {
        protected override DbSet<Artist> DbSet => context.Artists;

        public ArtistDAL(ScreenSoundContext context) : base(context) { }

        public List<Artist> GetByName(string name) => GetByNameAsync(name).Result;
        public async Task<List<Artist>> GetByNameAsync(string name)
        {
            List<Artist> output = new();
            await foreach (Artist artist in context.Artists)
                if (artist.Name == name)
                    output.Add(artist);
            return output;
        }

        public Artist? GetFirstByName(string name) => GetFirstByNameAsync(name).Result;
        public async Task<Artist?> GetFirstByNameAsync(string name)
        {
            await foreach (Artist artist in context.Artists)
                if (artist.Name == name)
                    return artist;

            return null;
        }
    }
}
