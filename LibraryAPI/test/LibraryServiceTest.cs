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
        var createBookDto = new CreateBookDto(
            srcTitle: "New Book", srcAuthor: "New Author", srcIsbn: "9876543210123",
            publishedDateUtcDateTime: DateTime.Now);

        var expectedBookModel = new BookModel(
            id: 1,
            title: createBookDto.Title,
            author: createBookDto.Author,
            isbn: createBookDto.Isbn,
            publishedDate: createBookDto.PublishedDate
        );


        MockRepo.Setup(repo => repo.Add(It.IsAny<BookModel>()))
            .ReturnsAsync(expectedBookModel); // Simulate adding the book

        // Act
        var result = await _libraryService.AddBookAsync(createBookDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedBookModel.Title, result.Title);
        Assert.Equal(expectedBookModel.Author, result.Author);
        Assert.Equal(expectedBookModel.Isbn, result.Isbn);
        MockRepo.Verify(repo => repo.Add(It.IsAny<BookModel>()), Times.Once);
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

public class BaseServiceFixture
{
    protected readonly IMapper Mapper;
    protected Mock<IAsyncRepository<BookModel>> MockRepo = new();
    protected readonly Mock<ILogger<LibraryService.LibraryService>> MockLogger = new();

    protected BaseServiceFixture()
    {
        Mapper = AutoMapperFixture.MapperFactory();
    }
}