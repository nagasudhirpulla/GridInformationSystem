using App.Common.Behaviours;
using App.Common.Interfaces;
using App.Owners.Utils;
using Core.Entities;
using Core.Entities.Elements;
using Core.Enums;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Buses.Commands.CreateBus;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record CreateBusCommand : IRequest<int>
{
    public int SubstationId { get; set; }
    public required string OwnerIds { get; set; }
    public required string ElementNumber { get; set; }
    public DateTime CommissioningDate { get; set; }
    public DateTime? DeCommissioningDate { get; set; }
    public DateTime CommercialOperationDate { get; set; }
    public bool IsImportantGridElement { get; set; } = false;
    public BusTypeEnum BusType { get; set; } = null!;
}

public class CreateBusCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateBusCommand, int>
{
    public async Task<int> Handle(CreateBusCommand request, CancellationToken cancellationToken)
    {
        // query substation from db
        var substation = await context.Substations
            .Include(s => s.VoltageLevel)
            .FirstOrDefaultAsync(s => s.Id == request.SubstationId, cancellationToken) ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "Substation Id is not present in database"
                                                                                }]);

        // derive element name 
        string name = Utils.DeriveBusName.Execute(substation.NameCache, request.BusType, request.ElementNumber);

        // derive voltage level, region from substation
        string voltLvl = substation.VoltageLevel.Level;
        string region = substation.RegionCache;

        // derive owner names cache
        List<Owner> owners = await OwnerUtils.GetOwnersFromIdsAsync(request.OwnerIds, context, cancellationToken);
        string ownersNames = OwnerUtils.DeriveOwnersCache(owners);

        // insert bus to db
        var entity = new Bus()
        {
            BusType = request.BusType,
            ElementNameCache = name,
            VoltLevelCache = voltLvl,
            RegionCache = region,
            SubstationId1 = request.SubstationId,
            OwnerNamesCache = ownersNames,
            ElementNumber = request.ElementNumber,
            CommissioningDate = request.CommissioningDate,
            DeCommissioningDate = request.DeCommissioningDate,
            CommercialOperationDate = request.CommercialOperationDate,
            IsImportantGridElement = request.IsImportantGridElement
        };

        context.Buses.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        // create ElementOwners entries
        await OwnerUtils.InsertElementOwnersAsync(entity.Id, owners.Select(o => o.Id), context, cancellationToken);

        return entity.Id;
    }
}
