using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.LibraryService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LibraryAPI.LibraryService;

public class BookRepository : RepositoryBase<BookModel>, IBookRepository
{
    private readonly DbContext Context;
    private ILogger<BookRepository> _logger;

    public BookRepository(DevAppDbContext context, ILogger<BookRepository> logger) : base(context) 
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(context));
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public ValueTask<BookModel?> GetById(int id)
    {
        return new ValueTask<BookModel?>(FindByCondition(b => b.Id == id).FirstOrDefaultAsync());
    }
    
    public Task<BookModel> Add(BookModel entity)
    {
        Create(entity);
        return Task.FromResult(entity);
    }

    public void Update(BookModel entity)
    {
        base.Update(entity);
    }

    public async Task<IEnumerable<BookModel>> GetAll()
    {
       return await FindAll().OrderBy((b) => b.Title).ToListAsync();
    }
}

public interface IBookRepository
{
    ValueTask<BookModel?> GetById(int id);

    Task<BookModel> Add(BookModel entity);


    void Update(BookModel entity);

    Task<IEnumerable<BookModel>> GetAll();

}