using System;

namespace LibraryAPI.LibraryService;

public class BaseEntity
{
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
}