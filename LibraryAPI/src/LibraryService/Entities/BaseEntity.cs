using System;

namespace LibraryAPI.LibraryService.Entities;

public class BaseEntity(Guid keyId, int id) : IEntity
{
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
    public Guid KeyId { get; set; } = keyId;
    public int Id { get; set; } = id;  // use Id for now but want to have an internal Guid as well for flexibilty later
}