using Core.Entities.Common;

namespace Core.Entities;

public class SubstationOwner : AuditableEntity
{
    public required Owner Owner { get; set; }
    public int OwnerId { get; set; }

    public required Substation Substation { get; set; }
    public int SubstationId { get; set; }
}