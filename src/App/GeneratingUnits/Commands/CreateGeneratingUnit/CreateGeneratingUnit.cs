using App.Common.Behaviours;
using App.Common.Interfaces;
using App.Owners.Utils;
using Core.Entities;
using Core.Entities.Elements;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.GeneratingUnits.Commands.CreateGeneratingUnit;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record CreateGeneratingUnitCommand : IRequest<int>
{
    public int GeneratingStationId { get; set; }
    public required string OwnerIds { get; set; }
    public required string ElementNumber { get; set; }
    public DateTime CommissioningDate { get; set; }
    public DateTime? DeCommissioningDate { get; set; }
    public DateTime CommercialOperationDate { get; set; }
    public bool IsImportantGridElement { get; set; } = false;
    public double InstalledCapacity { get; set; }
    public double GeneratingVoltage { get; set; }
}

public class CreateGeneratingUnitCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateGeneratingUnitCommand, int>
{
    public async Task<int> Handle(CreateGeneratingUnitCommand request, CancellationToken cancellationToken)
    {
        // query generating station from db
        var substation = await context.GeneratingStations
            .Include(s => s.VoltageLevel)
            .FirstOrDefaultAsync(s => s.Id == request.GeneratingStationId, cancellationToken) ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() { ErrorMessage = "Generating Station Id is not present in database" }]);

        // derive element name 
        string name = Utils.DeriveGenUnitName.Execute(substation.Name, request.ElementNumber);

        // derive voltage level, region from substation
        string voltLvl = substation.VoltageLevel.Level;
        string region = substation.RegionCache;

        // derive owner names cache
        List<Owner> owners = await OwnerUtils.GetOwnersFromIdsAsync(request.OwnerIds, context, cancellationToken);
        string ownersNames = OwnerUtils.DeriveOwnersCache(owners);

        // insert element to db
        var entity = new GeneratingUnit()
        {
            Name = name,
            VoltageLevelCache = voltLvl,
            RegionCache = region,
            Substation1Id = request.GeneratingStationId,
            OwnerNamesCache = ownersNames,
            ElementNumber = request.ElementNumber,
            CommissioningDate = request.CommissioningDate,
            DeCommissioningDate = request.DeCommissioningDate,
            CommercialOperationDate = request.CommercialOperationDate,
            IsImportantGridElement = request.IsImportantGridElement,
            Capacity = request.InstalledCapacity,
            GeneratingVoltage = request.GeneratingVoltage
        };

        context.GeneratingUnits.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        // create ElementOwners entries
        await OwnerUtils.InsertElementOwnersAsync(entity.Id, owners.Select(o => o.Id), context, cancellationToken);

        return entity.Id;
    }
}