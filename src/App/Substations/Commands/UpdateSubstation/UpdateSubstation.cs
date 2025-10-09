using App.Common.Behaviours;
using App.Common.Interfaces;
using App.Owners.Utils;
using App.Substations.Utils;
using Ardalis.GuardClauses;
using Core.Entities;
using Core.Events.Substations;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Substations.Commands.UpdateSubstation;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record UpdateSubstationCommand : IRequest
{
    public int Id { get; init; }
    public int VoltageLevelId { get; set; }
    public bool IsAc { get; set; } = true;
    public required string OwnerIds { get; init; }
    public int LocationId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class UpdateSubstationCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateSubstationCommand>
{
    public async Task Handle(UpdateSubstationCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Substations
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        // update entity attributes
        entity.Latitude = request.Latitude;
        entity.Longitude = request.Longitude;

        // update location if required
        bool? isElementsConnected = null;
        bool isLocationChangeRequested = entity.LocationId != request.LocationId;
        bool isVoltageChangeRequested = entity.VoltageLevelId != request.VoltageLevelId;
        bool isAcChangeRequested = entity.IsAc != request.IsAc;
        bool isNameChangeRequired = isVoltageChangeRequested | isAcChangeRequested;

        if (isLocationChangeRequested || isVoltageChangeRequested || isAcChangeRequested)
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

        // update ownerIds if required
        var existingSubstationOwnerRecords = await context.SubstationOwners.Where(s => s.SubstationId == request.Id).ToListAsync(cancellationToken: cancellationToken);
        var existingOwnerIds = existingSubstationOwnerRecords.Select(o => o.OwnerId).ToList();
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

            foreach (SubstationOwner substationOwner in existingSubstationOwnerRecords.Where(so => ownerIdsToDelete.Contains(so.OwnerId)))
            {

                context.SubstationOwners.Remove(substationOwner);
            }

            // update ownerNames cache
            List<Owner> newOwners = await context.Owners.Where(o => newOwnerIds.Contains(o.Id)).ToListAsync(cancellationToken: cancellationToken);
            var ownersCache = OwnerUtils.DeriveOwnersCache(newOwners);
            entity.OwnerNamesCache = ownersCache;
        }

        // set new substation name if required
        if (isNameChangeRequired)
        {
            var newName = await SubstationUtils.DeriveSubstationName(request.VoltageLevelId, request.LocationId, context, cancellationToken);
            entity.Name = newName;
            entity.AddDomainEvent(new SubstationNameChangedEvent(entity));
        }

        // save changes to DB
        await context.SaveChangesAsync(cancellationToken);
    }
}
