using System;
using System.ComponentModel.DataAnnotations;
using LibraryAPI.LibraryService.Entities;

namespace LibraryAPI.LibraryService.Models;

public class BookModel : BaseEntity
{
    public BookModel(string srcTitle, string srcAuthor, string srcIsbn, DateTime srcPublishedDate) 
    {
        Title = srcTitle;
        Author = srcAuthor;
        Isbn = srcIsbn;
        PublishedDate = srcPublishedDate;
    }

    public BookModel(int id, string title, string author, string isbn, DateTime publishedDate) : base(new Guid(), id)
    {
        Id = id;
        PublishedDate = publishedDate;
        Isbn = isbn;
        Author = author;
        Title = title;
    }

    public DateTime PublishedDate { get; set; }

    public string Isbn { get; set; }

    public string Author { get; set; }

    public string Title { get; set; }
}