using Microsoft.EntityFrameworkCore;
using screensound.database.dal;
using screensound.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace screensound.database
{
    public class MusicDAL : DAL<Music, ScreenSoundContext>
    {
        protected override DbSet<Music> DbSet => context.Musics;

        public MusicDAL(ScreenSoundContext context) : base(context) { }

        public List<Music> GetByName(string name) => GetByNameAsync(name).Result;
        public async Task<List<Music>> GetByNameAsync(string name)
        {
            List<Music> output = new();
            await foreach (Music artist in context.Musics)
                if (artist.Name == name)
                    output.Add(artist);
            return output;
        }

        public Music? GetFirstByName(string name) => GetFirstByNameAsync(name).Result;
        public async Task<Music?> GetFirstByNameAsync(string name)
        {
            await foreach (Music artist in context.Musics)
                if (artist.Name == name)
                    return artist;

            return null;
        }
    }
}
