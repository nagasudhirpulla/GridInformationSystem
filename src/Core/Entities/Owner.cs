namespace Core.Entities;

public class Owner : GridEntity
{
    public List<SubstationOwner> SubstationOwners { get; } = [];
    public List<ElementOwner> ElementOwners { get; } = [];
}
