using System;
using AutoMapper;

namespace LibraryAPI.LibraryService.Models;

public class BookMapperProfile : Profile
{
    public BookMapperProfile()
    {
        CreateMap<BookModel, BookWithId>()
            .ConstructUsing(src =>
                new BookWithId(src.Id, src.Title, src.Author, src.Isbn, new DateTimeOffset(src.PublishedDate, TimeSpan.Zero)));
        
        CreateMap<CreateBookModel, Book>()
            .ConstructUsing(src => new Book(src.Title, src.Author, src.Isbn, DateTimeOffset.Now));
        

        CreateMap<Book, CreateBookModel>()
            .ForMember(dest => dest.PublishedDate, opt => opt.MapFrom(src => src.PublishedDate.UtcDateTime));

        CreateMap<BookModel, Book>()
            .ConstructUsing(src => new Book(src.Title, src.Author, src.Isbn, src.PublishedDate));
    }
}

// Custom converter from DateTime to DateTimeOffset
public class DateTimeToDateTimeOffsetConverter : IValueConverter<DateTime, DateTimeOffset>
{
    public DateTimeOffset Convert(DateTime source, ResolutionContext context)
    {
        return new DateTimeOffset(source, TimeSpan.Zero); // Converts to UTC by default
    }
}

// Optional: Custom converter from DateTimeOffset to DateTime
public class DateTimeOffsetToDateTimeConverter : IValueConverter<DateTimeOffset, DateTime>
{
    public DateTime Convert(DateTimeOffset source, ResolutionContext context)
    {
        return source.UtcDateTime;
    }
}