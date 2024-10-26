using System;
using AutoMapper;
using LibraryAPI.LibraryService;
using LibraryAPI.LibraryService.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace LibraryAPI.test;

public class AutoMapperTest
{
    [Fact]
    public void ValidateAutoMapperConfiguration()
    {
        // Set up a service provider with AutoMapper for testing
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddAutoMapper(typeof(Startup)); // Register AutoMapper with all profiles from Startup

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var mapper = serviceProvider.GetRequiredService<IMapper>();

        // Act & Assert
        mapper.ConfigurationProvider.AssertConfigurationIsValid();

        ValidateBookModelToBookWithIdMapping(mapper);
    }

    private static void ValidateBookModelToBookWithIdMapping(IMapper mapper)
    {
        var now = DateTime.Now;
        
        BookModel bookModel = new BookModel(1, "Title", "Author", "Isbn", now);
        BookWithId expectedBookWithId = new BookWithId(bookModel.Id, bookModel.Title, bookModel.Author, bookModel.Isbn, bookModel.PublishedDate);
        var actualBookWithId = mapper.Map<BookWithId>(bookModel);
        
        Assert.Equivalent(expectedBookWithId, actualBookWithId);
    }
}