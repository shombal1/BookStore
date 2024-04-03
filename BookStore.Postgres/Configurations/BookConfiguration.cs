using BookStore.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Postgres.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<BookEntity>
    {
        public void Configure(EntityTypeBuilder<BookEntity> builder)
        {
            builder.HasKey(key => key.Id); // primary key

            builder.HasOne(key => key.Author)
                .WithMany(key=>key.Books)
                .HasForeignKey(key=>key.AuthorId);
        }
    }
}
