using AutoMapper;
using LibraryAPI.LibraryService.Entities.Dtos;
using LibraryAPI.LibraryService.Models;

namespace LibraryAPI.LibraryService.Entities.Models;

public class BookMapperProfile : Profile
{
    public BookMapperProfile()
    {
        CreateMap<BookModel, BookWithId>()
            .ConstructUsing(src =>
                new BookWithId(src.Id, src.Title, src.Author, src.Isbn, src.PublishedDate));

        CreateMap<CreateBookDto, Book>()
            .ConstructUsing(src => new Book(src.Title, src.Author, src.Isbn, src.PublishedDate));


        CreateMap<Book, CreateBookDto>()
            .ConstructUsing(src => new CreateBookDto(src.Title, src.Author, src.Isbn, src.PublishedDate.UtcDateTime))
            .ForMember(dest => dest.PublishedDate, opt => opt.Ignore());

        CreateMap<BookModel, Book>()
            .ConstructUsing(src => new Book(src.Title, src.Author, src.Isbn, src.PublishedDate));
    }
}
