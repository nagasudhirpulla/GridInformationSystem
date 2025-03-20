using App.Common.Behaviours;
using App.Lines.Commands.UpdateLine;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.LineReactors.Commands.UpdateLineReactor;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record UpdateLineReactorCommand : IRequest
{
    public int Id { get; set; }
    public required string OwnerIds { get; set; }
    public required string ElementNumber { get; set; }
    public DateTime CommissioningDate { get; set; }
    public DateTime? DeCommissioningDate { get; set; }
    public DateTime CommercialOperationDate { get; set; }
    public bool IsImportantGridElement { get; set; } = false;
    public int SubstationId { get; set; }
    public int LineId { get; set; }
    public double MvarCapacity { get; set; }
    public bool IsConvertible { get; set; }
    public bool IsSwitchable { get; set; }
}
