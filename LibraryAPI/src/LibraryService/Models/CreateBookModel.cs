using System;

namespace LibraryAPI.LibraryService.Models;

public class CreateBookModel : BaseEntity
{

    public CreateBookModel()
    {
        
    }

    public CreateBookModel(string title, string author, string isbn, DateTime publishedDate)
    {
        Title = title;
        Author = author;
        Isbn = isbn;
        PublishedDate = publishedDate;
    }

    public string Title { get; set; }
    public string Author { get; set; }
    public string Isbn { get; set; }
    public DateTime PublishedDate { get; set; }
}