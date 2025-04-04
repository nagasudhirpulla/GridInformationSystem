using App.Common.Behaviours;
using Core.Enums;
using MediatR;

namespace App.HvdcPoles.Commands.UpdateHvdcPole;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record UpdateHvdcPoleCommand : IRequest
{
    public int Id { get; set; }
    public int SubstationId { get; set; }
    public required string OwnerIds { get; set; }
    public required string ElementNumber { get; set; }
    public DateTime CommissioningDate { get; set; }
    public DateTime? DeCommissioningDate { get; set; }
    public DateTime CommercialOperationDate { get; set; }
    public bool IsImportantGridElement { get; set; } = false;
    public HvdcPoleTypeEnum PoleType { get; set; } = null!;
}
