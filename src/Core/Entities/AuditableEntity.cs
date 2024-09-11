namespace Core.Entities;

public class AuditableEntity : BaseEntity
{
    public required ApplicationUser CreatedBy { get; set; }
    public required string CreatedById { get; set; }

    public DateTime Created { get; set; } = DateTime.Now;

    public required ApplicationUser LastModifiedBy { get; set; }
    public required string LastModifiedById { get; set; }

    public DateTime? LastModified { get; set; }
}