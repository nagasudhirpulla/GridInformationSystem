using App.Common.Interfaces;
using App.Owners.Utils;
using App.Substations.Utils;
using Ardalis.GuardClauses;
using Core.Entities;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Substations.Commands.UpdateSubstation;
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
        // TODO use transactions
        var entity = await context.Substations
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Latitude = request.Latitude;
        entity.Longitude = request.Longitude;

        // update location if required
        if (entity.LocationId != request.LocationId)
        {
            Location location = await context.Locations.FirstOrDefaultAsync(l => l.Id == request.LocationId, cancellationToken)
                                ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "Location Id is not present in database"
                                                                                }]);
            entity.LocationId = request.LocationId;
            entity.RegionCache = location.RegionCache;
        }

        bool? isElementsConnected = null;

        // update voltage level if required
        if (entity.VoltageLevelId != request.VoltageLevelId)
        {
            VoltageLevel voltageLevel = await context.VoltageLevels.FirstOrDefaultAsync(l => l.Id == request.VoltageLevelId, cancellationToken)
                                ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "Voltage level Id is not present in database"
                                                                                }]);

            isElementsConnected ??= await SubstationUtils.IsElementsConnected(request.Id, context, cancellationToken);

            if (isElementsConnected == true)
            {
                throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "Can't change substation voltage level when there are connected elements"
                                                                                }]);
            }

            entity.VoltageLevelId = request.VoltageLevelId;
        }

        // update isAc if required
        if (entity.IsAc != request.IsAc)
        {
            isElementsConnected ??= await SubstationUtils.IsElementsConnected(request.Id, context, cancellationToken);

            if (isElementsConnected == true)
            {
                throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "Can't change substation AC DC of when there are connected elements"
                                                                                }]);
            }

            entity.IsAc = request.IsAc;
        }

        // update ownerIds if required
        var existingSubstationOwnerRecords = await context.SubstationOwners.Where(s => s.SubstationId == request.Id).ToListAsync(cancellationToken: cancellationToken);
        var existingOwnerIds = existingSubstationOwnerRecords.Select(o => o.OwnerId).ToList();
        var newOwnerIds = request.OwnerIds.Split(',').Select(o => int.Parse(o)).ToList();

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
        if ((request.VoltageLevelId != entity.VoltageLevelId) || (request.LocationId != entity.LocationId))
        {
            var newName = await SubstationUtils.DeriveSubstationName(request.VoltageLevelId, request.LocationId, context, cancellationToken);
            entity.NameCache = newName;
        }

        // save changes to DB
        await context.SaveChangesAsync(cancellationToken);
    }
}
