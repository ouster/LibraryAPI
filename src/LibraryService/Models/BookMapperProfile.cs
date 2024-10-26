using System;
using AutoMapper;

namespace LibraryAPI.LibraryService.Models;

public class BookMapperProfile : Profile
{
    public BookMapperProfile()
    {
        CreateMap<BookModel, BookWithId>()
            
            .ForMember(dest => dest.Id, opt => opt.MapFrom<int>(src => src.Id) )
            .ForMember(dest => dest.Title, opt => opt.MapFrom<string>(src => src.Title) )
            .ForMember(dest => dest.Author, opt => opt.MapFrom<string>(src => src.Author) )
            .ForMember(dest => dest.Isbn, opt => opt.MapFrom<string>(src => src.Isbn) )
            .ForMember(dest => dest.PublishedDate, opt => opt.MapFrom(src => new DateTimeOffset(src.PublishedDate, TimeSpan.Zero)));
            ;
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