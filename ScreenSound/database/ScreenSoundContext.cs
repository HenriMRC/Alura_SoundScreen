using Microsoft.EntityFrameworkCore;
using screensound.models;

namespace screensound.database
{
    internal class ScreenSoundContext : DbContext
    {
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(CONNECTION_STRING);
        }
    }
}
