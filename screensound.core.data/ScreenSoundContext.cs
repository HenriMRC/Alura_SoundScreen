using Microsoft.EntityFrameworkCore;
using screensound.core.models;

namespace screensound.core.data
{
    public class ScreenSoundContext : DbContext
    {
        public const string DEFAULT_CONNECTION_STRING =
            "Data Source=(localdb)\\MSSQLLocalDB;" +
            "Initial Catalog=ScreenSoundV0;" +
            "Integrated Security=True;" +
            //"Connect Timeout=30;" +
            "Encrypt=False;" +
            "Trust Server Certificate=False;" +
            "Application Intent=ReadWrite;" +
            "Multi Subnet Failover=False";

        public DbSet<Artist> Artists { get; set; }
        public DbSet<Music> Musics { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public ScreenSoundContext()
        {

        }

        public ScreenSoundContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.Options.Extensions.Any(e => e.Info.IsDatabaseProvider))
                optionsBuilder.UseSqlServer(DEFAULT_CONNECTION_STRING);

            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
