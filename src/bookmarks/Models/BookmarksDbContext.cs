using Microsoft.EntityFrameworkCore;

namespace bookmarks.Models
{
    public class BookmarksDbContext : DbContext
    {
        public BookmarksDbContext(DbContextOptions options) : base(options)
        {
        }

        protected BookmarksDbContext()
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
    }
}