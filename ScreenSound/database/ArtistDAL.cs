using Microsoft.EntityFrameworkCore.ChangeTracking;
using screensound.models;
using System.Collections.Generic;
using System.Linq;

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
