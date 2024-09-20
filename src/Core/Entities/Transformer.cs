namespace Core.Entities;

/*
 * transformer type can be ICT, GT
 * TODO make transformer type as Enum or use foreign key
 */

public class Transformer : AuditableEntity
{
    public required string TransformerType { get; set; }
    public double MvaCapacity { get; set; }
}
