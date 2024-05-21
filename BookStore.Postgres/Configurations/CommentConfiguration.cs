using BookStore.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Postgres.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<CommentEntity>
{
    public void Configure(EntityTypeBuilder<CommentEntity> builder)
    {
        builder.HasKey(key => key.Id);

        builder.HasOne(key => key.Author)
            .WithMany(key => key.Comments)
            .HasForeignKey(key=>key.AuthorId);

        builder.HasOne(key => key.Book)
            .WithMany(key => key.Comments)
            .HasForeignKey(key => key.BookId);

        builder.HasOne(key => key.ReplyComment)
            .WithMany(key => key.ReplayComments)
            .HasForeignKey(key => key.ReplyCommentId);
    }
}