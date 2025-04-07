using App.Common.Behaviours;
using MediatR;

namespace App.FilterBanks.Commands.CreateFilterBank;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record CreateFilterBankCommand : IRequest<int>
{
    public int SubstationId { get; set; }
    public required string OwnerIds { get; set; }
    public required string ElementNumber { get; set; }
    public DateTime CommissioningDate { get; set; }
    public DateTime? DeCommissioningDate { get; set; }
    public DateTime CommercialOperationDate { get; set; }
    public bool IsImportantGridElement { get; set; } = false;
    public double Mvar { get; set; }
    public bool IsSwitchable { get; set; }
}
