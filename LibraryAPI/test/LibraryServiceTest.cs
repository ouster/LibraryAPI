using System;
using System.Linq;
using AutoMapper;
using LibraryAPI.LibraryService;
using LibraryAPI.LibraryService.Entities.Dtos;
using LibraryAPI.LibraryService.Models;
using Microsoft.Extensions.Logging;

namespace LibraryAPI.test;

using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class LibraryServiceTests : BaseServiceFixture
{
    private readonly LibraryService.LibraryService _libraryService;

    public LibraryServiceTests()
    {
        _libraryService = new LibraryService.LibraryService(MockRepo.Object, Mapper, MockLogger.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsBook_WhenBookExists()
    {
        // Arrange
        var bookId = 1;
        var book = new BookModel(id: bookId, title: "Test Book", author: "Test Author", isbn: "1234567890123",
            publishedDate: DateTime.Now);

        MockRepo.Setup(repo => repo.GetById(bookId)).ReturnsAsync(book);

        // Act
        var result = await _libraryService.GetBookAsync(bookId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(bookId, result.Id);
        Assert.Equal("Test Book", result.Title);
    }

    [Fact]
    public async Task AddAsync_CreatesBook()
    {
        // Arrange
        var bookDto = new CreateBookDto(
             srcTitle : "New Book", srcAuthor : "New Author", srcIsbn : "9876543210123", publishedDateUtcDateTime : DateTime.Now );
        var bookModel = Mapper.Map<BookModel>(bookDto);

        MockRepo.Setup(repo => repo.Add(bookModel)).ReturnsAsync(bookModel); // Simulate adding the book

        // Act
        var result = await _libraryService.AddBookAsync(bookDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Book", result.Title);
        MockRepo.Verify(repo => repo.Add(bookModel), Times.Once); // Verify the AddAsync method was called once
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllBooks()
    {
        // Arrange
        var books = new List<BookModel>
        {
            new BookModel(id: 1, title: "Book One", author: "Author One", isbn: "1234567890123",
                publishedDate: DateTime.Now),
            new BookModel(id: 2, title: "Book Two", author: "Author Two", isbn: "9876543210123",
                publishedDate: DateTime.Now)
        };

        MockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(books);

        // Act
        var result = await _libraryService.GetBooksAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }
}

public class BaseServiceFixture : IDisposable
{
    protected readonly IMapper Mapper;
    protected Mock<IAsyncRepository<BookModel>> MockRepo = new();
    protected readonly Mock<ILogger<LibraryService.LibraryService>> MockLogger = new();

    protected BaseServiceFixture()
    {
        Mapper = AutoMapperFixture.MapperFactory();
    }

    public void Dispose()
    {
    }
}