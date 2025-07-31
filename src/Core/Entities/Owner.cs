namespace Core.Entities;

public class Owner : GridEntity
{
    public required string Name { get; set; }
    public List<SubstationOwner> SubstationOwners { get; } = [];
    public List<ElementOwner> ElementOwners { get; } = [];
}
