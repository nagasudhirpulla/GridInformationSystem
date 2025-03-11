using App.Common.Behaviours;
using Core.Enums;
using MediatR;

namespace App.Transformers.Commands.UpdateTransformer;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record UpdateTransformerCommand : IRequest
{
    public int Id { get; set; }
    public int Substation1Id { get; set; }
    public int Substation2Id { get; set; }
    public required string OwnerIds { get; set; }
    public required string ElementNumber { get; set; }
    public DateTime CommissioningDate { get; set; }
    public DateTime? DeCommissioningDate { get; set; }
    public DateTime CommercialOperationDate { get; set; }
    public bool IsImportantGridElement { get; set; } = false;
    public TransformerTypeEnum TransformerType { get; set; } = null!;
    public double MvaCapacity { get; set; }
    public string? Make { get; set; }
}

// TODO complete update handler
