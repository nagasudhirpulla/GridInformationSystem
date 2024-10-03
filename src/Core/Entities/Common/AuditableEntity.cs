namespace Core.Entities.Common;

public class AuditableEntity : BaseEntity
{
    public string? CreatedBy { get; set; } = null!;

    public DateTimeOffset Created { get; set; }

    public string? LastModifiedBy { get; set; } = null!;

    public DateTimeOffset LastModified { get; set; }
}