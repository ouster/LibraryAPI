
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

    /// <summary>
    /// Get all books
    /// </summary>

    /// <remarks>
    /// Retrieve a list of all books in the library.
    /// </remarks>

    /// <returns>OK</returns>

    System.Threading.Tasks.Task<System.Collections.Generic.ICollection<BookModel>> GetBooksAsync();

    /// <summary>
    /// Add a new book
    /// </summary>

    /// <remarks>
    /// Add a new book to the library.
    /// </remarks>

    /// <returns>Created</returns>

    System.Threading.Tasks.Task<BookModel> PostBookAsync(Book body);

    /// <summary>
    /// Get a specific book
    /// </summary>

    /// <remarks>
    /// Retrieve a specific book by its ID.
    /// </remarks>

    /// <returns>OK</returns>

    Task<BookModel?> GetBookAsync(int id);

    /// <summary>
    /// Update a book
    /// </summary>

    /// <remarks>
    /// Update an existing book by its ID.
    /// </remarks>



    /// <returns>OK</returns>

    System.Threading.Tasks.Task<BookModel> PutBookAsync(int id, Book body);

}

public interface IBookService
{
    Task<ICollection<BookModel>> GetBooksAsync();
}

public class LibraryDbService : ILibraryService
{
    private readonly DevAppDbContext _context;
    private readonly ILogger<LibraryDbService> _logger;

    public LibraryDbService(DevAppDbContext context, ILogger<LibraryDbService> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public async Task<ICollection<BookModel>> GetBooksAsync()
    {
        return await _context.Books
            .Select(b => new BookModel
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Isbn = b.Isbn,
                PublishedDate = b.PublishedDate
            })
            .ToListAsync();
    }

    public Task<BookModel> PostBookAsync(Book body)
    {
        throw new NotImplementedException();
    }

    public async Task<BookModel?> GetBookAsync(int id)
    {
        var book = await _context.Books
            .Where(b => b.Id == id) // Filter by ID
            .Select(b => new BookModel
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Isbn = b.Isbn,
                PublishedDate = b.PublishedDate
            })
            .FirstOrDefaultAsync();
        
        if (book != null) return book;

        var msg = $"Book with ID {id} not found.";
        _logger.LogError(msg);
        
        throw new KeyNotFoundException(msg);
    }

    public Task<BookModel> PutBookAsync(int id, Book body)
    {
        throw new NotImplementedException();
    }
}