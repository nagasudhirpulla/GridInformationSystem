namespace Core.Entities.Common;

public class AuditableEntity : BaseEntity
{
    public ApplicationUser CreatedBy { get; set; } = null!;
    public required string CreatedById { get; set; }

    public DateTime Created { get; set; } = DateTime.Now;

    public ApplicationUser LastModifiedBy { get; set; } = null!;
    public required string LastModifiedById { get; set; }

    public DateTime? LastModified { get; set; }
}