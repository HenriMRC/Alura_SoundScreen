using Microsoft.EntityFrameworkCore;
using screensound.models;

namespace screensound.database
{
    public class ScreenSoundContext : DbContext
    {
        private readonly string _connectionString;
        private const string CONNECTION_STRING =
            "Data Source=(localdb)\\MSSQLLocalDB;" +
            "Initial Catalog=ScreenSound;" +
            "Integrated Security=True;" +
            //"Connect Timeout=30;" +
            "Encrypt=False;" +
            "Trust Server Certificate=False;" +
            "Application Intent=ReadWrite;" +
            "Multi Subnet Failover=False";

        public DbSet<Artist> Artists { get; set; }
        public DbSet<Music> Musics { get; set; }

        public ScreenSoundContext()
        {
            _connectionString = CONNECTION_STRING;
        }

        public ScreenSoundContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
