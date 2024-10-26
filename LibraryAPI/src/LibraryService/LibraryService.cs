
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.LibraryService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LibraryAPI.LibraryService;

[System.CodeDom.Compiler.GeneratedCode("NSwag", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
public interface ILibraryService
{
    Task<IEnumerable<BookModel>> GetBooksAsync();

    Task<BookModel?> AddBookAsync(CreateBookModel body);

    Task<BookModel?> GetBookAsync(int id);

    Task<BookModel> UpdateBookAsync(int id, CreateBookModel body);

}

public class LibraryService : ILibraryService
{ //    private readonly DevAppDbContext _context;
    private readonly IAsyncRepository<BookModel> _bookRepository;
    private readonly ILogger<LibraryService> _logger;

    public LibraryService(IAsyncRepository<BookModel> bookRepository, ILogger<LibraryService> logger)
    {
        _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(_bookRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public async Task<IEnumerable<BookModel>> GetBooksAsync()
    {

        return await _bookRepository.GetAll();

    }

    public async Task<BookModel> AddBookAsync(CreateBookModel createBook)
    {
        if (createBook == null)
        {
            throw new ArgumentNullException(nameof(createBook), "Book cannot be null.");
        }

        var book = LibraryService.createBook(createBook);

        return await _bookRepository.Add(book);
    }

    
    public async Task<BookModel?> GetBookAsync(int id)
    {
        var book = await _bookRepository.GetById(id);
        if (book != null) return book;

        var msg = $"Book with ID {id} not found.";
        _logger.LogError(msg);
        
        throw new KeyNotFoundException(msg);
    }

    public async Task<BookModel> UpdateBookAsync(int id, CreateBookModel updatedBook)
    {
        if (updatedBook == null)
        {
            throw new ArgumentNullException(nameof(updatedBook), "Updated book cannot be null.");
        }
        var existingBook = await _bookRepository.GetById(id);
        if (existingBook == null)
        {
            throw new KeyNotFoundException($"Unable to update Book with ID {id} not found.");
        }

        // Update properties
        existingBook.Title = updatedBook.Title;
        existingBook.Author = updatedBook.Author;
        existingBook.Isbn = updatedBook.Isbn;
        existingBook.PublishedDate = updatedBook.PublishedDate;

        // Save the changes to the database
        await _bookRepository.Update(existingBook);

        return existingBook;
    }

    private static BookModel createBook(CreateBookModel createBook)
    {
        var book = new BookModel();
        book.Title = createBook.Title;
        book.Author = createBook.Author;
        book.Isbn = createBook.Isbn;
        book.PublishedDate = createBook.PublishedDate;
        return book;
    }
}