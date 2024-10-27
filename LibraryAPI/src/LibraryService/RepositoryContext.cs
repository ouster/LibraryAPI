using LibraryAPI.LibraryService.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.LibraryService;

public abstract class RepositoryContext : DbContext
{
    protected RepositoryContext(DbContextOptions options) 
        : base(options) 
    { 
    }
}