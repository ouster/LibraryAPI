namespace LibraryAPI.test;

public class AsyncEfRespositoryTest
{
    
}

//using System;
// using LibraryAPI.LibraryService;
// using LibraryAPI.LibraryService.Models;
// using Microsoft.Extensions.Logging;
// using Moq;
// 
// namespace LibraryAPI.test;
// 
// using Microsoft.EntityFrameworkCore;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Xunit;
// 
// public class LibraryServiceTests : IDisposable
// {
//     private readonly DevAppDbContext _context;
//     private readonly LibraryService.LibraryService _libraryService;
// 
//     public LibraryServiceTests()
//     {
//         // Setup the in-memory database
//         var options = new DbContextOptionsBuilder<DevAppDbContext>()
//             // TODO mocking db context for results tricky so can add a seperate db layer later and mock that
//             .UseInMemoryDatabase(databaseName: "UnitTestDatabase")
//             .Options;
// 
//         var mockLogger = new Mock<ILogger<LibraryService.LibraryService>>();
//         _context = new DevAppDbContext(options);
//         var mockRepo = new Mock<IAsyncRepository<BookModel>>();
//         _libraryService = new LibraryService.LibraryService(mockRepo.Object, mockLogger.Object);
//     }
// 
//     [Fact]
//     public async Task GetBooksAsync_ReturnsAllBooks()
//     {
//         // Arrange: Seed some test data
//         var books = new List<BookModel>
//         {
//             new BookModel()
//             {
//                 Id = 1, Title = "Book One", Author = "Author One", Isbn = "1234567890123", PublishedDate = DateTime.Now
//             },
//             new BookModel()
//             {
//                 Id = 2, Title = "Book Two", Author = "Author Two", Isbn = "0987654321", PublishedDate = DateTime.Now
//             }
//         };
// 
//         _context.Books.AddRange(books);
//         await _context.SaveChangesAsync();
// 
//         // Act: Call the method
//         var result = await _libraryService.GetBooksAsync();
// 
//         // Assert: Check if the result matches the expected
//         Assert.NotNull(result);
//         Assert.Equal(2, result.Count());
//         Assert.Equal("Book One", result.ElementAt(0).Title as string);
//         Assert.Equal("Book Two", result.ElementAt(1).Title as string);
//     }
// 
//     [Fact]
//     public async Task GetBookAsync_ReturnsBook_WhenBookExists()
//     {
//         // Arrange: Seed a single book
//         var book = new BookModel()
//             { Id = 1, Title = "Book One", Author = "Author One", Isbn = "1234567890", PublishedDate = DateTime.Now };
//         _context.Books.Add(book);
//         await _context.SaveChangesAsync();
// 
//         // Act: Call the method
//         var result = await _libraryService.GetBookAsync(1);
// 
//         // Assert: Check if the returned book matches
//         Assert.NotNull(result);
//         Assert.Equal("Book One", result.Title as string);
//         Assert.Equal("Author One", result.Author as string);
//     }
// 
//     [Fact]
//     public async Task GetBookAsync_ReturnsNull_WhenBookDoesNotExist()
//     {
//         // Act: Call the method with a non-existing ID
//         try
//         {
//             var result = await _libraryService.GetBookAsync(999); // Non-existing ID
//             Assert.Fail();
//         }
//         catch (KeyNotFoundException e)
//         {
//         }
//     }
//     
//     [Fact]
//     public async Task AddBookAsync_AddsBookSuccessfully()
//     {
//         // Arrange: Create a new book model
//         var newBook = new BookModel
//         {
//             Id = 3,
//             Title = "New Book",
//             Author = "New Author",
//             Isbn = "1122334455667",
//             PublishedDate = DateTime.Now
//         };
// 
//         // Act: Add the new book using the service
//         await _libraryService.AddBookAsync(newBook);
// 
//         // Assert: Verify the book was added
//         var addedBook = await _libraryService.GetBookAsync(newBook.Id);
//         Assert.NotNull(addedBook);
//         Assert.Equal("New Book", addedBook.Title);
//         Assert.Equal("New Author", addedBook.Author);
//     }
// 
//     [Fact]
//     public async Task UpdateBookAsync_UpdatesBookSuccessfully()
//     {
//         // Arrange: Seed a book to update
//         var bookToUpdate = new BookModel
//         {
//             Id = 1,
//             Title = "Old Title",
//             Author = "Old Author",
//             Isbn = "9876543210987",
//             PublishedDate = DateTime.Now
//         };
//         _context.Books.Add(bookToUpdate);
//         await _context.SaveChangesAsync();
// 
//         // Act: Update the book's details
//         bookToUpdate.Title = "Updated Title";
//         bookToUpdate.Author = "Updated Author";
//         await _libraryService.UpdateBookAsync(1, bookToUpdate);
// 
//         // Assert: Verify the book details were updated
//         var updatedBook = await _libraryService.GetBookAsync(bookToUpdate.Id);
//         Assert.NotNull(updatedBook);
//         Assert.Equal("Updated Title", updatedBook.Title);
//         Assert.Equal("Updated Author", updatedBook.Author);
//     }
// 
// 
//     public void Dispose()
//     {
//         _context.Database.EnsureDeleted();
//         _context.Dispose();
//     }
// }