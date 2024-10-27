using LibraryAPI.LibraryService.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.LibraryService;

public class RepositoryContext : DbContext
{
    public RepositoryContext(DbContextOptions options) 
        : base(options) 
    { 
    }
}