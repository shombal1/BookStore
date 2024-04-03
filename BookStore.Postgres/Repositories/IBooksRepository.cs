using BookStore.Postgres.Models;

namespace BookStore.Postgres.Repositories
{
    public interface IBooksRepository
    {
        public Task<Guid> Add(Guid authorId,string title, string description, decimal price);
        public Task<List<BookEntity>> GetAll();
        public Task Delete(Guid id);
        public Task Update(Guid id, string newTitle, string newDescription, decimal newPrice);
    }
}