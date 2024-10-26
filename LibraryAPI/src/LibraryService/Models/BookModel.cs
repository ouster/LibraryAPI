using System;

namespace LibraryAPI.LibraryService.Models;

public class BookModel : CreateBookModel
{
    public BookModel()
    {
    }

    public BookModel(int id, string title, string author, string isbn, DateTime publishedDate) : base(title, author, isbn, publishedDate)
    {
        Id = id;
        Title = title;
        Author = author;
        Isbn = isbn;
        PublishedDate = publishedDate;
    }

    public int Id { get; set; }

}