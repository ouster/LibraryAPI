using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.LibraryService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LibraryAPI.LibraryService;

public class BookRepository : IBookRepository
{
    private readonly DevAppDbContext _context;
    private ILogger<BookRepository> _logger;


    public BookRepository(DevAppDbContext context, ILogger<BookRepository> logger) 
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(context));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async ValueTask<BookModel?> GetById(int id)
    {
        return await _context.Books.FindAsync(id);
    }

    public async Task<BookModel> Add(BookModel entity)
    {
        var addedEntity = await _context.Books.AddAsync(entity);
        await _context.SaveChangesAsync();
        return addedEntity.Entity; // Return the added entity
    }

    public void Update(BookModel entity)
    {
        _context.Books.Update(entity);
        _context.SaveChanges(); // Save changes synchronously
    }

    public async Task<IEnumerable<BookModel>> GetAll()
    {
        return await _context.Books
            .OrderBy(b => b.Title)
            .ToListAsync();
    }
    
    public async Task SeedBooksAsync()
    {
        // Check if the database is already seeded
        if (_context.Books.Any())
        {
            _logger.LogWarning("DB already seeded");
            return;
        }

        var books = new List<BookModel>
        {
            new BookModel(id: 1, title: "1984", author: "George Orwell", isbn: "9781234567897", publishedDate: new DateTime(1949, 6, 8)),
            new BookModel(id: 2, title: "To Kill a Mockingbird", author: "Harper Lee", isbn: "9783127323207", publishedDate: new DateTime(1960, 7, 11)),
            new BookModel(id: 3, title: "The Great Gatsby", author: "F. Scott Fitzgerald", isbn: "4855186747734", publishedDate: new DateTime(1925, 4, 10)),
            new BookModel(id: 4, title: "Brave New World", author: "Aldous Huxley", isbn: "4501169518", publishedDate: new DateTime(1932, 1, 1))
        };

        await _context.Books.AddRangeAsync(books);
        await _context.SaveChangesAsync();
    }
}

public interface IBookRepository
{
    ValueTask<BookModel?> GetById(int id);

    Task<BookModel> Add(BookModel entity);


    void Update(BookModel entity);

    Task<IEnumerable<BookModel>> GetAll();

}