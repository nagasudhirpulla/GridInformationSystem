using App.Common.Behaviours;
using MediatR;

namespace App.Lines.Commands.CreateLine;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record CreateLineCommand : IRequest<int>
{
    public int Bus1Id { get; set; }
    public int Bus2Id { get; set; }
    public required string OwnerIds { get; set; }
    public required string ElementNumber { get; set; }
    public DateTime CommissioningDate { get; set; }
    public DateTime? DeCommissioningDate { get; set; }
    public DateTime CommercialOperationDate { get; set; }
    public bool IsImportantGridElement { get; set; } = false;
    public double Length { get; set; }
    public required string ConductorType { get; set; }
    public bool IsAutoReclosurePresent { get; set; }
}
