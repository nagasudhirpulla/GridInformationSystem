namespace Core.Entities;

/*
 * bus type can be main, transfer, auxilliary
 * TODO make bus type as Enum or use foreign key
 */

public class Bus : AuditableEntity
{
    public required string BusType { get; set; }
}
