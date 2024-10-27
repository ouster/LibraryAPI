using System;
using LibraryAPI.LibraryService.Models;

namespace LibraryAPI.LibraryService.Entities.Dtos;

public class UpdateBookDto(string srcTitle, string srcAuthor, string srcIsbn, DateTime publishedDateUtcDateTime) : CreateBookDto(srcTitle, srcAuthor, srcIsbn, publishedDateUtcDateTime)
{
    
}