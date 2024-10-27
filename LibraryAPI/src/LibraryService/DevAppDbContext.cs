using System;
using Microsoft.EntityFrameworkCore;
using LibraryAPI.LibraryService.Models;

namespace LibraryAPI.LibraryService;

public class DevAppDbContext(DbContextOptions options) : DbContext(options)
{
    // Move to integration test context later
    public DbSet<BookModel> Books { get; set; }

    public const int TitleMaxLength = 100;
    public const int AuthorMaxLength = 50;
    public const int IsbnMaxLength = 20; // future expansion, may change in the future (was 10 until 2007)

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BookModel>()
            .Property(b => b.Title)
            .HasMaxLength(TitleMaxLength);

        modelBuilder.Entity<BookModel>()
            .Property(b => b.Author)
            .HasMaxLength(AuthorMaxLength);

        modelBuilder.Entity<BookModel>()
            .Property(b => b.Isbn)
            .HasMaxLength(IsbnMaxLength);

        // Seed initial test data
        modelBuilder.Entity<BookModel>().HasData(
            new BookModel(id: 1, title: "1984", author: "George Orwell", isbn: "9781234567897",
                publishedDate: new DateTime(1949, 6, 8)),
            new BookModel(id: 2, title: "To Kill a Mockingbird", author: "Harper Lee", isbn: "9783127323207",
                publishedDate: new DateTime(1960, 7, 11)),
            new BookModel(id: 2, title: "The Great Gatsby", author:  "F. Scott Fitzgerald", isbn: "4855186747734",
                publishedDate: new DateTime(1925, 4, 10)),
            new BookModel(id: 2, title: "Brave New World", author: "Aldous Huxley", isbn: "4501169518",
                publishedDate: new DateTime(1932, 1, 1))
        );
    }
}