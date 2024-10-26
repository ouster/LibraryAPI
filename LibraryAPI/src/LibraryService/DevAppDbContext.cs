using System;
using Microsoft.EntityFrameworkCore;
using LibraryAPI.LibraryService.Models;

namespace LibraryAPI.LibraryService;

public class DevAppDbContext(DbContextOptions options) : DbContext(options)
{
    // Define your DbSets (tables) here
    public DbSet<BookModel> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BookModel>()
            .Property(b => b.Title)
            .HasMaxLength(100);

        modelBuilder.Entity<BookModel>()
            .Property(b => b.Author)
            .HasMaxLength(50);

        modelBuilder.Entity<BookModel>()
            .Property(b => b.Isbn)
            .HasMaxLength(13); // may change in the future (was 10 until 2007)

        // Seed initial test data
        modelBuilder.Entity<BookModel>().HasData(
            new BookModel
            {
                Id = 1, Title = "1984", Author = "George Orwell", Isbn = "9781234567897",
                PublishedDate = new DateTime(1949, 6, 8),
            },
            new BookModel
            {
                Id = 2, Title = "To Kill a Mockingbird", Author = "Harper Lee", Isbn = "9783127323207",
                PublishedDate = new DateTime(1960, 7, 11),
            },
            new BookModel
            {
                Id = 3, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Isbn = "4855186747734",
                PublishedDate = new DateTime(1925, 4, 10)
            },
            new BookModel
            {
                Id = 4, Title = "Brave New World", Author = "Aldous Huxley", Isbn = "4501169518",
                PublishedDate = new DateTime(1932, 1, 1)
            }
        );
    }
}