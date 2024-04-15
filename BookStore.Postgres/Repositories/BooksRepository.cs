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
                .Include(key => key.Author)
                .ToListAsync();
        }

        public async Task<List<BookEntity>> GetPageLastAdded(int numberPage, int sizePage)
        {
            return await _dbContext.Books.AsNoTracking()
                .OrderByDescending(key => key.PublicationDate)
                .Skip(numberPage * sizePage)
                .Take(sizePage)
                .Include(key => key.Author)
                .ToListAsync();
        }

        public async Task<List<BookEntity>> GetPageHighestRating(int numberPage, int sizePage)
        {
            return await _dbContext.Books.AsNoTracking()
                .OrderByDescending(key => key.Rating)
                .Skip(numberPage * sizePage)
                .Take(sizePage)
                .Include(key => key.Author)
                .ToListAsync();
        }

        public async Task Update(Guid id, string newTitle, string newDescription, decimal newPrice,
            DateTimeOffset publicationDate, double rating)
        {
            await _dbContext.Books.Where(key => key.Id == id)
                .ExecuteUpdateAsync(val =>
                    val.SetProperty(key => key.Title, newTitle)
                        .SetProperty(key => key.Description, newDescription)
                        .SetProperty(key => key.Price, newPrice)
                        .SetProperty(key => key.PublicationDate, publicationDate)
                        .SetProperty(key=>key.Rating,rating)
                );

            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            await _dbContext.Books
                .Where(key => key.Id == id)
                .ExecuteDeleteAsync();

            await _dbContext.SaveChangesAsync();
        }

        public async Task<Guid> Add(Guid authorId, string title, string description, decimal price,
            DateTimeOffset publicationDate, double rating)
        {
            Guid id = Guid.NewGuid();
            await _dbContext.Books.AddAsync(new BookEntity
            {
                Id = id,
                Title = title,
                Description = description,
                AuthorId = authorId,
                Price = price,
                PublicationDate = publicationDate,
                Rating = rating
            });

            await _dbContext.SaveChangesAsync();

            return id;
        }
    }
}