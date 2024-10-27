using System;

namespace LibraryAPI.LibraryService.Entities;

public interface IEntity
{
    Guid KeyId { get; set; }
    int Id { get; set; }
}