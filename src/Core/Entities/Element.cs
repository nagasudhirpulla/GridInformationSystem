namespace Core.Entities;

public class Element : AuditableEntity
{
    public int ElementNameCache { get; set; }

    public int SubstationId1 { get; set; }
    public required Substation Substation1 { get; set; }

    public int SubstationId2 { get; set; }
    public required Substation Substation2 { get; set; }

    public int ElementTypeId { get; set; }
    public required ElementType ElementType { get; set; }

    public required string OwnerNamesCache { get; set; }
    public required string ElementNumber { get; set; }
    public DateTime CommissioningDate { get; set; }
    public DateTime DeCommissioningDate { get; set; }
    public DateTime CommercialOperationDate { get; set; }

    public int LocationId { get; set; }
    public required Location Location { get; set; }
}
