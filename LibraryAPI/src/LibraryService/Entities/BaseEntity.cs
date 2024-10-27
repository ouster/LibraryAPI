using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.LibraryService.Entities;

public class BaseEntity : IEntity
{
    protected BaseEntity()
    {
    }

    protected BaseEntity(Guid? keyId, int id)
    {
        KeyId = keyId ?? Guid.NewGuid();
        Id = id;
    }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
    public Guid KeyId { get; set; } = Guid.NewGuid();
    
    [Key]
    public int Id { get; set; } // use Id for now but want to have an internal Guid as well for flexibilty later
}