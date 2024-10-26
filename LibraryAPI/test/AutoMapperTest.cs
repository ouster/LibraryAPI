using System;
using AutoMapper;
using LibraryAPI.LibraryService;
using LibraryAPI.LibraryService.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace LibraryAPI.test;

public class AutoMapperTest : IDisposable
{
    private readonly IMapper _mapper;

    public AutoMapperTest()
    {
        _mapper = SetupMapper();
    }

    [Fact]
    public void ValidateAutoMapperConfiguration()
    {
        // Act & Assert
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    private IMapper SetupMapper()
    {
        // Set up a service provider with AutoMapper for testing
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddAutoMapper(typeof(Startup)); // Register AutoMapper with all profiles from Startup

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var mapper = serviceProvider.GetRequiredService<IMapper>();
        return mapper;
    }

    [Fact]
    public void ValidateBookModelToBookWithIdMapping()
    {
        var now = DateTime.Now;
        
        var bookModel = new BookModel(1, "Title", "Author", "Isbn", now);
        var expectedBookWithId = new BookWithId(1, bookModel.Title, bookModel.Author, bookModel.Isbn, bookModel.PublishedDate);
        var actualBookWithId = _mapper.Map<BookWithId>(bookModel);
        
        Assert.Equal(expectedBookWithId.Id, actualBookWithId.Id);
        Assert.Equal(expectedBookWithId.Title, actualBookWithId.Title);
        Assert.Equal(expectedBookWithId.Author, actualBookWithId.Author);
        Assert.Equal(expectedBookWithId.Isbn, actualBookWithId.Isbn);
        Assert.Equal(expectedBookWithId.PublishedDate, actualBookWithId.PublishedDate);
    }
    
    [Fact]
    public void ValidateBookToBookModelMapping()
    {
        var now = DateTime.Now;
        
        var book = new Book("Title", "Author", "Isbn", now);
        var expectedBookModel = new Book(book.Title, book.Author, book.Isbn, book.PublishedDate);
        var actualBookModel = _mapper.Map<CreateBookModel>(book);
        
        Assert.Equal(expectedBookModel.Title, actualBookModel.Title);
        Assert.Equal(expectedBookModel.Author, actualBookModel.Author);
        Assert.Equal(expectedBookModel.Isbn, actualBookModel.Isbn);
        Assert.Equal(expectedBookModel.PublishedDate, actualBookModel.PublishedDate);
    }

    public void Dispose()
    {
        
    }
}