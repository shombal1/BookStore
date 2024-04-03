using BookStore.Postgres.Configurations;
using BookStore.Postgres.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Postgres
{
    public class BookStoreDbContext : DbContext
    {
        public BookStoreDbContext( DbContextOptions<BookStoreDbContext> options ): base(options)
        {
                
        }

        public DbSet<BookEntity> Books { get; set; }
        public DbSet<AuthorEntity> Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new AuthorConfiguration());
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
