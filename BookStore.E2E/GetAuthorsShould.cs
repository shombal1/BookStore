using System.Text;
using System.Text.Json;
using BookStore.APi.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

namespace E2E;

public class GetAuthorsShould : IClassFixture<BookStoreWebApplicationFactory>
{
    private readonly BookStoreWebApplicationFactory _factory;

    public GetAuthorsShould(BookStoreWebApplicationFactory factory)
    {
        _factory = factory;
    }
    
    [Fact]
    public async Task GetAuthor()
    {
        using var client = _factory.CreateClient();

        var response = await client.GetAsync("Authors/All");
        
        response.Invoking(r => r.EnsureSuccessStatusCode()).Should().NotThrow();

        string result = await response.Content.ReadAsStringAsync();
        result.Should().BeEquivalentTo("[]");
    }
}