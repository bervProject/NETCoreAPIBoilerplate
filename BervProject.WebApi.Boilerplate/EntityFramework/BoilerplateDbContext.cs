using BervProject.WebApi.Boilerplate.Entities;
using Microsoft.EntityFrameworkCore;

namespace BervProject.WebApi.Boilerplate.EntityFramework
{
    public class BoilerplateDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Publisher> Publishers { get; set; }

        public BoilerplateDbContext() : base()
        {

        }

        public BoilerplateDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Book>().HasOne(m => m.Publisher).WithMany(m => m.Books);
        }
    }
}
