using Microsoft.EntityFrameworkCore;
using TestWebApiOnlineMove.Models;

namespace TestWebApiOnlineMove.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Video> Videos { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }
}
