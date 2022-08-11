using Microsoft.EntityFrameworkCore;
using Task1.Models;

namespace Task1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Line> Lines { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public void Dispose()
        {
            Thread.Sleep(1000_000);
        }
    }
}
