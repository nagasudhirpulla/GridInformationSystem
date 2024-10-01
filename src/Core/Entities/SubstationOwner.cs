using Core.Entities.Common;

namespace Core.Entities;

public class SubstationOwner : AuditableEntity
{
    public Owner Owner { get; set; } = null!;
    public int OwnerId { get; set; }

    public Substation Substation { get; set; } = null!;
    public int SubstationId { get; set; }
}