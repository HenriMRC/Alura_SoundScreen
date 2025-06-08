using Microsoft.EntityFrameworkCore.ChangeTracking;
using screensound.models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace screensound.database
{
    internal class ArtistDAL
    {
        private readonly ScreenSoundContext _context;

        public ArtistDAL(ScreenSoundContext context)
        {
            _context = context;
        }

        public List<Artist> GetList() => _context.Artists.ToList();

        public List<Artist> GetByName(string name) => GetByNameAsync(name).Result;
        public async Task<List<Artist>> GetByNameAsync(string name)
        {
            List<Artist> output = new();
            await foreach (Artist artist in _context.Artists)
                if (artist.Name == name)
                    output.Add(artist);
            return output;
        }

        public Artist? GetFirstByName(string name) => GetFirstByNameAsync(name).Result;
        public async Task<Artist?> GetFirstByNameAsync(string name)
        {
            await foreach (Artist artist in _context.Artists)
                if (artist.Name == name)
                    return artist;

            return null;
        }

        public EntityEntry<Artist> Add(Artist artist)
        {
            EntityEntry<Artist> result = _context.Artists.Add(artist);
            _context.SaveChanges();
            return result;
        }

        public EntityEntry<Artist> Update(Artist artist)
        {
            EntityEntry<Artist> result = _context.Artists.Update(artist);
            _context.SaveChanges();
            return result;
        }

        public EntityEntry<Artist> Remove(Artist artist)
        {
            EntityEntry<Artist> result = _context.Artists.Update(artist);
            _context.SaveChanges();
            return result;
        }
    }
}
