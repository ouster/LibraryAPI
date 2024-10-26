using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.LibraryService;

public class AsyncEfRepository<T> : IAsyncRepository<T>
    where T : BaseEntity
{
    private readonly DbContext _context;

    public AsyncEfRepository(DevAppDbContext context) // TODO
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public ValueTask<T?> GetById(int id) => _context.Set<T>().FindAsync(id);

    public Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate)
        => _context.Set<T>().FirstOrDefaultAsync(predicate);

    public async Task<T> Add(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
        
        return entity;
    }

    public Task Update(T entity)
    {
        // In case AsNoTracking is used
        _context.Entry(entity).State = EntityState.Modified;
        return _context.SaveChangesAsync();
    }

    public Task Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
        return _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().Where(predicate).ToListAsync();
    }

    public Task<int> CountAll() => _context.Set<T>().CountAsync();

    public Task<int> CountWhere(Expression<Func<T, bool>> predicate)
        => _context.Set<T>().CountAsync(predicate);
    
}