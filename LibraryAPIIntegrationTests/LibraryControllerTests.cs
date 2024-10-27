using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using LibraryAPI;
using LibraryAPI.LibraryService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Program = LibraryAPI.Program;

namespace LibraryAPIIntegrationTests;

public class LibraryControllerTests(WebApplicationFactory<Program> factory) : ApiTestFixture(factory)
{
    [Fact]
    public async Task GetBooks_ReturnsOk_WhenBooksExist()
    {
        // Act
        var response = await Client.GetAsync($"{BaseUrl}/books");

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
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/book", newBook);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var createdBook = await response.Content.ReadFromJsonAsync<BookWithId>();
        createdBook.Should().NotBeNull();
        createdBook?.Title.Should().Be(newBook.Title);
        createdBook?.Author.Should().Be(newBook.Author);
        createdBook?.Id.Should().BePositive();
    }

    [Fact]
    public async Task GetBook_ReturnsOk_WhenBookExists()
    {
        // Arrange: Assume you have a book with ID 1 already in the database
        int bookId = 1;

        // Act
        var response = await Client.GetAsync($"{BaseUrl}/book/{bookId}");

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
        var response = await Client.GetAsync($"{BaseUrl}/book/999"); // Assuming ID 999 does not exist

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
        var response = await Client.PutAsJsonAsync($"{BaseUrl}/book/{bookId}", updatedBook);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var book = await response.Content.ReadFromJsonAsync<BookWithId>();
        book.Should().NotBeNull();
        book?.Title.Should().Be(updatedBook.Title);
        book?.Author.Should().Be(updatedBook.Author);
    }
}