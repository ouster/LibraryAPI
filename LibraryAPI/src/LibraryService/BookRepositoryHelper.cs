using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.LibraryService.Models;
using Microsoft.Extensions.Logging;

namespace LibraryAPI.LibraryService;

public static class BookRepositoryHelper
{
    public static List<BookModel> GenerateBookList()
    {
        var books = new List<BookModel>
        {
            new BookModel(id: 1, title: "1984", author: "George Orwell", isbn: "9781234567897", publishedDate: new DateTime(1949, 6, 8)),
            new BookModel(id: 2, title: "To Kill a Mockingbird", author: "Harper Lee", isbn: "9783127323207", publishedDate: new DateTime(1960, 7, 11)),
            new BookModel(id: 3, title: "The Great Gatsby", author: "F. Scott Fitzgerald", isbn: "4855186747734", publishedDate: new DateTime(1925, 4, 10)),
            new BookModel(id: 4, title: "Brave New World", author: "Aldous Huxley", isbn: "4501169518", publishedDate: new DateTime(1932, 1, 1))
        };
        return books;
    }
}