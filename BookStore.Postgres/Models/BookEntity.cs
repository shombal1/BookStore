namespace BookStore.Postgres.Models
{
    public class BookEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Price { get; set; }
        public DateTimeOffset PublicationDate { get; set; } = DateTimeOffset.Now;
        public double Rating { get; set; } = 0;
        public Guid AuthorId { get; set; }
        public AuthorEntity? Author { get; set; }
    }
}