
using LibraryAPI.LibraryService.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.LibraryService;

public interface IBookService
{
    Task<ICollection<BookModel>> GetBooksAsync();
}

public class LibraryService(DevAppDbContext context) : ILibraryService
{
    public List<BookModel> GetBooks()
    {
        return context.Books.ToList();
    }
    
    public async Task<ICollection<BookModel>> GetBooksAsync()
    {
        return await context.Books
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
        return await context.Books
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
    }

    public Task<BookModel> PutBookAsync(int id, Book body)
    {
        throw new NotImplementedException();
    }
}