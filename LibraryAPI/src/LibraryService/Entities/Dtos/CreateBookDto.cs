using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.LibraryService.Entities.Dtos;

public class CreateBookDto
{
    public CreateBookDto(string srcTitle, string srcAuthor, string srcIsbn, DateTime publishedDateUtcDateTime)
    {
        Title = srcTitle;
        Author = srcAuthor;
        Isbn = srcIsbn;
        PublishedDate = publishedDateUtcDateTime;
    }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, ErrorMessage = "Title can't be longer than 100 characters")]
    public string Title { get; set; }
    [Required(ErrorMessage = "Author is required")]
    [StringLength(50, ErrorMessage = "Author can't be longer than 50 characters")]
    public string Author { get; set; }
    [Required(ErrorMessage = "Isbn is required")]
    [StringLength(13, ErrorMessage = "Isbn can't be longer than 13 characters")]
    public string Isbn { get; set; }
    [Required(ErrorMessage = "PublishedDate is required")]
    public DateTime PublishedDate { get; set; }
}

