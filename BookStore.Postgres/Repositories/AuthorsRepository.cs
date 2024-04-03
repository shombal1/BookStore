using System.Diagnostics;
using BookStore.Postgres.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Postgres.Repositories;

public class AuthorsRepository:IAuthorsRepository
{
    private readonly BookStoreDbContext _dbContext;
        
    public AuthorsRepository(BookStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<AuthorEntity>> GetAll()
    {
        return await _dbContext.Authors.AsNoTracking()
            .Include(key=>key.Books)
            .ToListAsync();
    }

    public async Task<Guid> Add(string firstName,string lastName,string? patronymic,string city)
    {
        Guid id = Guid.NewGuid();

        AuthorEntity newAuthor = new AuthorEntity()
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName,
            Patronymic = patronymic,
            City = city
        };
        
        await _dbContext.Authors.AddAsync(newAuthor);

        await _dbContext.SaveChangesAsync();

        return id;
    }

    public async Task Update(Guid id,string firstName,string lastName,string? patronymic,string city)
    {
        await _dbContext.Authors.Where(key => key.Id == id)
            .ExecuteUpdateAsync(val => val
                .SetProperty(key=>key.FirstName,firstName)
                .SetProperty(key=>key.LastName,lastName)
                .SetProperty(key=>key.Patronymic,patronymic)
                .SetProperty(key=>key.City,city)
            );
        
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task Delete(Guid id)
    {
        _dbContext.Authors.Remove
        (
            await _dbContext.Authors.AsNoTracking()
                .FirstAsync(key => key.Id == id)
        );

        await _dbContext.SaveChangesAsync();
    }
}