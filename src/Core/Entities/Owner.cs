using Core.Entities.Common;

namespace Core.Entities;

public class Owner : AuditableEntity
{
    public required string Name { get; set; }
    public List<SubstationOwner> SubstationOwners { get; } = [];
    public List<ElementOwner> ElementOwners { get; } = [];
}
