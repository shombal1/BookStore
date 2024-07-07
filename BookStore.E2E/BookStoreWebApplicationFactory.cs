
using BookStore.Postgres;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;

namespace E2E;

public class BookStoreWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public readonly PostgreSqlContainer postgreSqlContainer = new PostgreSqlBuilder().Build();
    public readonly RedisContainer redisContainer = new RedisBuilder().Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseConfiguration(
            new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>()
            {
                { "ConnectionStrings:BookStoreDbContext", postgreSqlContainer.GetConnectionString() },
                { "ConnectionStrings:Redis", redisContainer.GetConnectionString() }
            }!).Build());

        base.ConfigureWebHost(builder);
    }

    public async Task InitializeAsync()
    {
        await postgreSqlContainer.StartAsync();
        await redisContainer.StartAsync();

        string j = redisContainer.GetConnectionString();

        BookStoreDbContext dbContext = new BookStoreDbContext(new DbContextOptionsBuilder<BookStoreDbContext>()
            .UseNpgsql(postgreSqlContainer.GetConnectionString()).Options);

        await dbContext.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await postgreSqlContainer.DisposeAsync();
        await redisContainer.DisposeAsync();
    }
}