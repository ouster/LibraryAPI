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

        CreateMap<CreateBookDto, BookModel>()
            .ConstructUsing(src => new BookModel(src.Title, src.Author, src.Isbn, src.PublishedDate))
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
            .ForMember(dest => dest.KeyId, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore());;

        CreateMap<Book, CreateBookDto>()
            .ConstructUsing(src => new CreateBookDto(src.Title, src.Author, src.Isbn, src.PublishedDate.UtcDateTime))
            .ForMember(dest => dest.PublishedDate, opt => opt.Ignore());

        CreateMap<Book, UpdateBookDto>()
            .ConstructUsing(src => new UpdateBookDto(src.Title, src.Author, src.Isbn, src.PublishedDate.UtcDateTime))
            .ForMember(dest => dest.PublishedDate, opt => opt.Ignore());

        CreateMap<BookModel, Book>()
            .ConstructUsing(src => new Book(src.Title, src.Author, src.Isbn, src.PublishedDate));
    }
}
