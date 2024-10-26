using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using LibraryAPI.LibraryService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Program = LibraryAPI.Program;

namespace LibraryAPIIntegrationTests;

public class LibraryControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public LibraryControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Development"); // TODO review this later
            builder.ConfigureServices(services =>
            {
                // Replace the DB context with an in-memory database
                var descriptor = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(DbContextOptions<DevAppDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);
                
                services.AddDbContext<DevAppDbContext>(options =>
                    options.UseInMemoryDatabase("IntegrationTestDb"));
            });
        });
        _client = _factory.CreateClient();
    }
    [Fact]
    public async Task GetBooks_ReturnsOk_WhenBooksExist()
    {
        // Act
        var response = await _client.GetAsync("/api/books");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var books = await response.Content.ReadFromJsonAsync<ICollection<BookWithId>>();
        books.Should().NotBeNull();
        books.Should().HaveCountGreaterThan(0); // Assuming there are pre-seeded books
    }

    [Fact]
    public async Task PostBook_ReturnsCreated_WhenBookIsAdded()
    {
        // Arrange
        var newBook = new Book
        {
            Title = "New Book",
            Author = "New Author",
            Isbn = "1234567890",
            PublishedDate = DateTimeOffset.Now
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/book", newBook);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var createdBook = await response.Content.ReadFromJsonAsync<BookWithId>();
        createdBook.Should().NotBeNull();
        createdBook.Title.Should().Be(newBook.Title);
        createdBook.Author.Should().Be(newBook.Author);
    }

    [Fact]
    public async Task GetBook_ReturnsOk_WhenBookExists()
    {
        // Arrange: Assume you have a book with ID 1 already in the database
        int bookId = 1;

        // Act
        var response = await _client.GetAsync($"/api/book/{bookId}");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var book = await response.Content.ReadFromJsonAsync<BookWithId>();
        book.Should().NotBeNull();
        book?.Id.Should().Be(bookId);
    }

    [Fact]
    public async Task GetBook_ReturnsNotFound_WhenBookDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync("/api/book/999"); // Assuming ID 999 does not exist

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PutBook_ReturnsOk_WhenBookIsUpdated()
    {
        // Arrange: Assume you have a book with ID 1
        int bookId = 1;
        var updatedBook = new Book
        {
            Title = "Updated Book",
            Author = "Updated Author",
            Isbn = "0987654321",
            PublishedDate = DateTimeOffset.Now
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/book/{bookId}", updatedBook);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var book = await response.Content.ReadFromJsonAsync<BookWithId>();
        book.Should().NotBeNull();
        book.Title.Should().Be(updatedBook.Title);
        book.Author.Should().Be(updatedBook.Author);
    }
}