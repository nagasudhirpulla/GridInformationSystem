using App.Common.Behaviours;
using Core.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Bays.Commands.CreateBay;

/*
ConnectingElement1 and ConnectingElement2 should be present in the same substation
For a Tie Bay Element1 and Element2 would be Non-Bus
For a Main Bay exactly one of the element1 or element2 would be bus
For a Bus Coupler, Bus Sectionalizer, TBC Bay elment1 and element2 both are bus type
fix element1 as bus for main bay
TODO
spare bay column may be added

 */
[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public class CreateBayCommand : IRequest<int>
{
    public int SubstationId { get; set; }
    public int Element1Id { get; set; }
    public int Element2Id { get; set; }
    public required string OwnerIds { get; set; }
    public DateTime CommissioningDate { get; set; }
    public DateTime? DeCommissioningDate { get; set; }
    public DateTime CommercialOperationDate { get; set; }
    public bool IsImportantGridElement { get; set; } = false;
    public BayTypeEnum BayType { get; set; } = null!;
    public bool IsFuture { get; set; } = false;
}
