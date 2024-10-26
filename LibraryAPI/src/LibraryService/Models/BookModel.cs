using System;

namespace LibraryAPI.LibraryService.Models;

public class BookModel
{
    public BookModel()
    {
    }

    public BookModel(int id, string title, string author, string isbn, DateTime publishedDate)
    {
        Id = id;
        Title = title;
        Author = author;
        Isbn = isbn;
        PublishedDate = publishedDate;
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Isbn { get; set; }
    public DateTime PublishedDate { get; set; }
}