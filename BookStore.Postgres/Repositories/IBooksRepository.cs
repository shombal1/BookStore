using BookStore.Postgres.Models;

namespace BookStore.Postgres.Repositories
{
    public interface IBooksRepository
    {
        public Task<Guid> Add(string title, string description, string author, decimal price);
        public Task<List<BookEntity>> GetAllBooks();
        public Task<BookEntity> Update(Guid id,string newTitle,string newDescription,string newAuthor,decimal newPrice);
        public Task<BookEntity> Delete(Guid id);
    }
}