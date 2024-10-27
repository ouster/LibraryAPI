using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LibraryAPI.LibraryService.Models;
using Microsoft.Extensions.Logging;

namespace LibraryAPI.LibraryService;

public class DevAppDbContext(DbContextOptions options, ILogger<DevAppDbContext> logger) : RepositoryContext(options)
{
    public DbSet<BookModel> Books { get; set; }

    public const int TitleMaxLength = 100;
    public const int AuthorMaxLength = 50;
    public const int IsbnMaxLength = 20; // future expansion, may change in the future (was 10 until 2007)

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
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

    public void SeedData()
    {
        // Seed data
        Database.EnsureDeleted(); // Clear the database
        Database.EnsureCreated(); // Create a new instance

        AddRange(
            new BookModel(id: 1, title: "1984", author: "George Orwell", isbn: "9781234567897", publishedDate: new DateTime(1949, 6, 8)),
            new BookModel(id: 2, title: "To Kill a Mockingbird", author: "Harper Lee", isbn: "9783127323207", publishedDate: new DateTime(1960, 7, 11)),
            new BookModel(id: 3, title: "The Great Gatsby", author: "F. Scott Fitzgerald", isbn: "4855186747734", publishedDate: new DateTime(1925, 4, 10)),
            new BookModel(id: 4, title: "Brave New World", author: "Aldous Huxley", isbn: "4501169518", publishedDate: new DateTime(1932, 1, 1))
        );

        SaveChanges(); // Save changes after seeding
        var books = this.Books.ToList(); // Assuming 'Books' is your DbSet<BookModel>
        if (books.Count > 0)
        {
            logger.LogDebug($"{books.Count} records found in the database.");
        }

    }
}