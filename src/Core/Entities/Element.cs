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

/*
 if the element type is not a line or transformer substationID2 should be null
if the element type is transformer substationID1 and substationID2 should belong to the same location
if the element type is transmission line substationID1 and substationID2 should not belong to the same location
combination of substationID1, substationID2, elementType, elementNumber is unique for non bus elements
combination of substationID1, substationID2, elementNumber,busType is unique for bus element type

TODOs
FSC
 */
