using App.Common.Behaviours;
using App.Common.Interfaces;
using App.Owners.Utils;
using App.Substations.Utils;
using Ardalis.GuardClauses;
using Core.Entities;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.GeneratingStations.Commands.UpdateGeneratingStation;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record UpdateGeneratingStationCommand : IRequest
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public int VoltageLevelId { get; set; }
    public bool IsAc { get; set; } = true;
    public required string OwnerIds { get; init; }
    public int LocationId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double InstalledCapacity { get; set; }
    public double MvaCapacity { get; set; }
    public int GeneratingStationClassificationId { get; set; }
    public int GeneratingStationTypeId { get; set; }
    public int FuelId { get; set; }
}

public class UpdateGeneratingStationCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateGeneratingStationCommand>
{
    public async Task Handle(UpdateGeneratingStationCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.GeneratingStations
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        // update entity attributes
        entity.Latitude = request.Latitude;
        entity.Longitude = request.Longitude;
        entity.MvaCapacity = request.MvaCapacity;
        entity.InstalledCapacity = request.InstalledCapacity;
        entity.FuelId = request.FuelId;

        // update location if required
        bool? isElementsConnected = null;
        bool isLocationChangeRequested = entity.LocationId != request.LocationId;
        bool isVoltageChangeRequested = entity.VoltageLevelId != request.VoltageLevelId;
        bool isAcChangeRequested = entity.IsAc != request.IsAc;
        bool isNameChangeRequested = entity.Name != request.Name;

        if (isLocationChangeRequested || isVoltageChangeRequested || isAcChangeRequested || isNameChangeRequested)
        {
            isElementsConnected ??= await SubstationUtils.IsElementsConnected(request.Id, context, cancellationToken);

            if (isElementsConnected == true)
            {
                throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "Can't change substation voltage level or location or AC DC when there are connected elements"
                                                                                }]);
            }
        }

        if (isLocationChangeRequested)
        {
            Location location = await context.Locations.FirstOrDefaultAsync(l => l.Id == request.LocationId, cancellationToken)
                                ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "Location Id is not present in database"
                                                                                }]);
            entity.LocationId = request.LocationId;
            entity.RegionCache = location.RegionCache;
        }


        // update voltage level if required
        if (isVoltageChangeRequested)
        {
            VoltageLevel voltageLevel = await context.VoltageLevels.FirstOrDefaultAsync(l => l.Id == request.VoltageLevelId, cancellationToken)
                                ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "Voltage level Id is not present in database"
                                                                                }]);
            entity.VoltageLevelId = request.VoltageLevelId;
        }

        // update isAc if required
        if (isAcChangeRequested)
        {
            entity.IsAc = request.IsAc;
        }

        // set new substation name if required
        if (isNameChangeRequested)
        {
            entity.Name = request.Name;
            entity.Name = request.Name;
        }

        // update ownerIds if required
        var existingGeneratingStationOwnerRecords = await context.SubstationOwners.Where(s => s.SubstationId == request.Id).ToListAsync(cancellationToken: cancellationToken);
        var existingOwnerIds = existingGeneratingStationOwnerRecords.Select(o => o.OwnerId).ToList();
        var newOwnerIds = request.OwnerIds.Split(',').Select(int.Parse).ToList();

        var ownerIdsToAdd = newOwnerIds.Where(o => !existingOwnerIds.Contains(o)).ToList();
        var ownerIdsToDelete = existingOwnerIds.Where(o => !newOwnerIds.Contains(o)).ToList();

        if (ownerIdsToAdd.Count > 0 || ownerIdsToDelete.Count > 0)
        {
            foreach (var ownerId in ownerIdsToAdd)
            {
                context.SubstationOwners.Add(new SubstationOwner
                {
                    SubstationId = request.Id,
                    OwnerId = ownerId
                });
            }

            foreach (SubstationOwner substationOwner in existingGeneratingStationOwnerRecords.Where(so => ownerIdsToDelete.Contains(so.OwnerId)))
            {

                context.SubstationOwners.Remove(substationOwner);
            }

            // update ownerNames cache
            List<Owner> newOwners = await context.Owners.Where(o => newOwnerIds.Contains(o.Id)).ToListAsync(cancellationToken: cancellationToken);
            var ownersCache = OwnerUtils.DeriveOwnersCache(newOwners);
            entity.OwnerNamesCache = ownersCache;
        }

        // save changes to DB
        await context.SaveChangesAsync(cancellationToken);
    }
}