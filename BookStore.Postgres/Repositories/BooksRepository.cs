using BookStore.Postgres.Models;
using Microsoft.EntityFrameworkCore;


namespace BookStore.Postgres.Repositories
{
    public class BooksRepository : IBooksRepository
    {
        private readonly BookStoreDbContext _dbContext;
        
        public BooksRepository(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<BookEntity>> GetAll()
        {
            return await _dbContext.Books.AsNoTracking()
                .Include(key=>key.Author)
                .ToListAsync();
        }

        public async Task Update(Guid id,string newTitle, string newDescription, decimal newPrice)
        {
            await _dbContext.Books.Where(key => key.Id == id)
                .ExecuteUpdateAsync(val =>
                    val.SetProperty(key => key.Title,newTitle)
                    .SetProperty(key=>key.Description,newDescription)
                    .SetProperty(key=>key.Price,newPrice)
                );
            
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            _dbContext.Books.Remove
            (
                await _dbContext.Books.AsNoTracking()
                    .FirstAsync(bookEntity => bookEntity.Id == id)
            );

            await _dbContext.SaveChangesAsync();
        }

        public async Task<Guid> Add(Guid authorId,string title, string description, decimal price)
        {
            Guid id = Guid.NewGuid();
            await _dbContext.Books.AddAsync(new BookEntity
            {
                Id = id,
                Title = title,
                Description = description,
                AuthorId = authorId,
                Price = price
            });

           await _dbContext.SaveChangesAsync();

           return id;
        }
    }
}