using BookStore.Postgres.Models;

namespace BookStore.Postgres.Repositories
{
    public interface IBooksRepository
    {
        public Task<Guid> Add(Guid authorId, string title, string description, decimal price,
            DateTimeOffset publicationDate, double rating);
        public Task<List<BookEntity>> GetAll();
        public Task<List<BookEntity>> GetPageLastAdded(int numberPage, int sizePage);
        public Task<List<BookEntity>> GetPageHighestRating(int numberPage, int sizePage);
        public Task Delete(Guid id);
        public Task Update(Guid id, string newTitle, string newDescription, decimal newPrice,
            DateTimeOffset publicationDate,double rating);
    }
}