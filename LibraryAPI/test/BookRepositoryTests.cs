using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.LibraryService;
using LibraryAPI.LibraryService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace LibraryAPI.test;

public class BookRepositoryTests : IDisposable
{
    private readonly DevAppDbContext _dbContext;
    private readonly Mock<ILogger<BookRepository>> _mockLogger;
    private readonly BookRepository _repository;

    public BookRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<DevAppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestLibraryDb")
            .Options;
        

        _mockLogger = new Mock<ILogger<BookRepository>>();
        _dbContext = new DevAppDbContext(options, new Mock<ILogger<DevAppDbContext>>().Object);
        _dbContext.ClearDb();
        _repository = new BookRepository(_dbContext, _mockLogger.Object);
    }

    [Fact]
    public async Task GetById_ValidId_ReturnsBook()
    {
        // Arrange
        var book = new BookModel(id: 100, title: "Test Book", author: "Test Author", isbn: "1234567890",
            publishedDate: DateTime.Now);

        _dbContext.Books.Add(book);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetById(100);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(book, result);
    }

    [Fact]
    public async Task Add_ValidBook_AddsBookAndReturnsIt()
    {
        // Arrange
        var book = new BookModel(id: 1, title: "New Book", author: "New Author", isbn: "1234567890",
            publishedDate: DateTime.Now);

        // Act
        var result = await _repository.Add(book);

        // Assert
        Assert.Equal(book, result);
        Assert.Contains(await _dbContext.Books.ToListAsync(), b => b.Id == book.Id && b.Title == book.Title);
    }

    [Fact]
    public void Update_ValidBook_UpdatesBook()
    {
        // Arrange
        var book = new BookModel(id: 100, title: "Updated Book", author: "Updated Author", isbn: "1234567890",
            publishedDate: DateTime.Now);
        _dbContext.Books.Add(book);
        _dbContext.SaveChanges();

        // Act
        book.Title = "Updated Title";
        _repository.Update(book);

        // Assert
        var updatedBook = _dbContext.Books.Find(100);
        Assert.Equal("Updated Title", updatedBook?.Title);
    }

    [Fact]
    public async Task GetAll_ReturnsListOfBooks()
    {
        // Arrange
        var books = new List<BookModel>
        {
            new BookModel ( id: 100, title: "New Book 1", author: "New Author", isbn: "1234567890", publishedDate: DateTime.Now ),
            new BookModel ( id: 101, title: "New Book 2", author: "New Author", isbn: "1234567890", publishedDate: DateTime.Now ),
            new BookModel ( id: 102, title: "New Book 3", author: "New Author", isbn: "1234567890", publishedDate: DateTime.Now ),
            new BookModel ( id: 103, title: "New Book 4", author: "New Author", isbn: "1234567890", publishedDate: DateTime.Now ),
        };
        _dbContext.Books.AddRange(books);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetAll();

        // Assert
        var bookModels = result as BookModel[] ?? result.ToArray();
        Assert.Equal(4, bookModels.Count());
        Assert.Equal("New Book 1", bookModels[0].Title);
        Assert.Equal("New Book 2", bookModels[1].Title);
        Assert.Equal("New Book 3", bookModels[2].Title);
        Assert.Equal("New Book 4", bookModels[3].Title);
    }

    public void Dispose()
    {
    }
}