using App.Bays.Commands.UpdateBay;
using App.Common.Behaviours;
using Core.Enums;
using MediatR;

namespace App.Bays.Commands.UpdateBay;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record UpdateBayCommand : IRequest
{
    // TODO complete this
    public int Id { get; set; }
    public int Element1Id { get; set; }
    public int Element2Id { get; set; }
    public required string OwnerIds { get; set; }
    public DateTime CommissioningDate { get; set; }
    public DateTime? DeCommissioningDate { get; set; }
    public DateTime CommercialOperationDate { get; set; }
    public bool IsImportantGridElement { get; set; } = false;
    public BayTypeEnum BayType { get; set; } = null!;
}
