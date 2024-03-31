using BookStore.Postgres.Models;
using Microsoft.EntityFrameworkCore;


namespace BookStore.Postgres.Repositories
{
    public class BooksRepository : IBooksRepository
    {
        private readonly BookStoreDbContext _dbContext;
        
        public BooksRepository(BookStoreDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<List<BookEntity>> GetAllBooks()
        {
            return await _dbContext.Books.AsNoTracking().ToListAsync();
        }

        public async Task<BookEntity> Update(Guid id,string newTitle, string newDescription, string newAuthor, decimal newPrice)
        {
            BookEntity updateBookEntity = _dbContext.Update(
                await _dbContext.Books.AsNoTracking().FirstAsync
                (
                    bookEntity => bookEntity.Id == id
                )).Entity;

            updateBookEntity.Title = newTitle;
            updateBookEntity.Author = newAuthor;
            updateBookEntity.Description = newDescription;
            updateBookEntity.Price = newPrice;
            
            await _dbContext.SaveChangesAsync();
            
            return updateBookEntity;
        }

        public async Task<BookEntity> Delete(Guid id)
        {
            BookEntity deletedBookEntity = _dbContext.Books.Remove(
                await _dbContext.Books.AsNoTracking().FirstAsync
                    (
                        bookEntity => bookEntity.Id == id
                    )).Entity;

            await _dbContext.SaveChangesAsync();
            
            return deletedBookEntity;
        }

        public async Task<Guid> Add(string title, string description, string author, decimal price)
        {
            Guid id = Guid.NewGuid();
            _dbContext.Books.Add(new BookEntity
            {
                Id = id,
                Title = title,
                Description = description,
                Author = author,
                Price = price
            });

           await _dbContext.SaveChangesAsync();

           return id;
        }
    }
}