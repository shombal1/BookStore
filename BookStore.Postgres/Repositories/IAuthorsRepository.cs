using BookStore.Postgres.Models;

namespace BookStore.Postgres.Repositories;

public interface IAuthorsRepository
{
    public Task<List<AuthorEntity>> GetAll();
    public Task<Guid> Add(string firstName, string lastName, string? patronymic, string city);
    public Task Update(Guid id, string firstName, string lastName, string? patronymic, string city);
    public Task Delete(Guid id);
}