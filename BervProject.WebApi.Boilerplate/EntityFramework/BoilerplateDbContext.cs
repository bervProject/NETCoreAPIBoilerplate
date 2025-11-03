using BervProject.WebApi.Boilerplate.Entities;
using Microsoft.EntityFrameworkCore;

namespace BervProject.WebApi.Boilerplate.EntityFramework
{
    /// <summary>
    /// DB Context
    /// </summary>
    public class BoilerplateDbContext : DbContext
    {
        /// <summary>
        /// Books
        /// </summary>
        public DbSet<Book> Books { get; set; }
        /// <summary>
        /// Publishers
        /// </summary>
        public DbSet<Publisher> Publishers { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BoilerplateDbContext() : base()
        {

        }

        /// <summary>
        /// Constructor with DbContextOptions
        /// </summary>
        /// <param name="options">DbContextOptions</param>
        public BoilerplateDbContext(DbContextOptions options) : base(options)
        {

        }

        /// <summary>
        /// Adding relationship
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Book>().HasOne(m => m.Publisher).WithMany(m => m.Books);
        }
    }
}
