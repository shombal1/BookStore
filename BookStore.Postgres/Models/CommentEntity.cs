namespace BookStore.Postgres.Models;

public class CommentEntity
{
    public Guid Id { get; set; }
    
    public Guid AuthorId { get; set; }
    public AuthorEntity Author { get; set; }
    
    public Guid BookId { get; set; }
    public BookEntity Book { get; set; }
    
    public Guid? ReplyCommentId { get; set; }
    public CommentEntity ReplyComment { get; set; }
    
    public ICollection<CommentEntity> ReplayComments { get; set; }

    public string Text { get; set; } = "";
}