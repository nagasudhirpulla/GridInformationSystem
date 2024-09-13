using Core.Entities;

public class Element : AuditableEntity
{
    public int ElementNameCache { get; set; }

    public int SubstationId1 { get; set; }
    public required Substation Substation { get; set; }

    public int ElementTypeId { get; set; }
    public required ElementType ElementType { get; set; }

    public required string OwnerNameCache { get; set; }
    public required string ElementNumber { get; set; }
    public DateTime CommissionDate { get; set; }
    public DateTime DeCommissionDate { get; set; }
    public DateTime CommercialOperationDate { get; set; }

    public int LocationId { get; set; }
    public required Location Location { get; set; }
}
