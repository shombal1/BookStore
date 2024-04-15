//using System.Diagnostics;

using System.Text.Json;
using System.Text.Json.Serialization;
using BookStore.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace BookStore.Postgres.Repositories;

public class AuthorsRepository : IAuthorsRepository
{
    private readonly BookStoreDbContext _dbContext;
    private readonly IDatabase _redis;

    public AuthorsRepository(BookStoreDbContext dbContext, IConnectionMultiplexer muxer)
    {
        _dbContext = dbContext;
        _redis = muxer.GetDatabase();
    }

    public async Task<List<AuthorEntity>> GetAll()
    {
        return await _dbContext.Authors.AsNoTracking()
            .Include(key => key.Books)
            .ToListAsync();
    }

    public async Task<AuthorEntity> Get(Guid id)
    {
        RedisValue redisValue = _redis.StringGet(id.ToString());
        AuthorEntity result;
        
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve
        };

        if (redisValue.HasValue)
        {
            result = JsonSerializer.Deserialize<AuthorEntity>(redisValue.ToString(),
                jsonSerializerOptions) ?? throw new InvalidOperationException();
        }
        else
        {
            result = await _dbContext.Authors.AsNoTracking()
                .Include(key => key.Books)
                .FirstAsync(key => key.Id == id);

            _redis.StringSet(id.ToString(), JsonSerializer.Serialize(result,
                jsonSerializerOptions),TimeSpan.FromMinutes(5));
        }

        return result;
    }

    public async Task<Guid> Add(string firstName, string lastName, string? patronymic, string city)
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

    public async Task Update(Guid id, string firstName, string lastName, string? patronymic, string city)
    {
        _redis.KeyDelete(id.ToString());

        await _dbContext.Authors.Where(key => key.Id == id)
            .ExecuteUpdateAsync(val => val
                .SetProperty(key => key.FirstName, firstName)
                .SetProperty(key => key.LastName, lastName)
                .SetProperty(key => key.Patronymic, patronymic)
                .SetProperty(key => key.City, city)
            );

        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        _redis.KeyDelete(id.ToString());

        await _dbContext.Authors
            .Where(key => key.Id == id)
            .ExecuteDeleteAsync();

        await _dbContext.SaveChangesAsync();
    }
}