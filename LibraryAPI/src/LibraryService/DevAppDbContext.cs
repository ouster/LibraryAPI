using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LibraryAPI.LibraryService.Models;
using Microsoft.Extensions.Logging;

namespace LibraryAPI.LibraryService;

public class DevAppDbContext(DbContextOptions options, ILogger<DevAppDbContext> logger) : DbContext(options)
{
    public DbSet<BookModel> Books { get; set; }

    public const int TitleMaxLength = 100;
    public const int AuthorMaxLength = 50;
    public const int IsbnMaxLength = 20; // future expansion, may change in the future (was 10 until 2007)

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookModel>()
            .Property(b => b.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<BookModel>()
            .Property(b => b.Title)
            .HasMaxLength(TitleMaxLength);

        modelBuilder.Entity<BookModel>()
            .Property(b => b.Author)
            .HasMaxLength(AuthorMaxLength);

        modelBuilder.Entity<BookModel>()
            .Property(b => b.Isbn)
            .HasMaxLength(IsbnMaxLength);
    }

    public void ClearDB()
    {
        // Seed data
        Database.EnsureDeleted(); // Clear the database
        Database.EnsureCreated(); // Create a new instance
    }
}