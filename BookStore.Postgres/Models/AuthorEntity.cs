namespace BookStore.Postgres.Models;

public class AuthorEntity
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string? Patronymic { get; set; }
    public string? City { get; set; }
    public List<BookEntity> Books { get; set; }
    public ICollection<CommentEntity> Comments { get; set; }
} 