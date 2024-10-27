using System;
using LibraryAPI.LibraryService.Entities;

namespace LibraryAPI.LibraryService.Models;

public class BookModel(int id, string title, string author, string isbn, DateTime publishedDate)
    : BaseEntity(new Guid(), id)
{
    public DateTime PublishedDate { get; set; } = publishedDate;

    public string Isbn { get; set; } = isbn;

    public string Author { get; set; } = author;

    public string Title { get; set; } = title;
}