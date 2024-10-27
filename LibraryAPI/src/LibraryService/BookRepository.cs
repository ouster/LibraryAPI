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
    private DevAppDbContext _context;
    private ILogger<BookRepository> _logger;


    public BookRepository()
    {
        
    }
    public BookRepository(DevAppDbContext context, ILogger<BookRepository> logger) 
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(context));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public virtual async ValueTask<BookModel?> GetById(int id)
    {
        return await _context.Books.FindAsync(id);
    }

    public virtual async Task<BookModel> Add(BookModel entity)
    {
        var addedEntity = await _context.Books.AddAsync(entity);
        await _context.SaveChangesAsync();
        return addedEntity.Entity; // Return the added entity
    }

    public virtual void Update(BookModel entity)
    {
        _context.Books.Update(entity);
        _context.SaveChanges(); // Save changes synchronously
    }

    public virtual async Task<IEnumerable<BookModel>> GetAll()
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

        var books = BookRepositoryHelper.GenerateBookList();

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