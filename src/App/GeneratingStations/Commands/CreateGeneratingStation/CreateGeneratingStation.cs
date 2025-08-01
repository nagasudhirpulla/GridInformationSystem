using App.Common.Behaviours;
using App.Common.Interfaces;
using App.Owners.Utils;
using Core.Entities;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.GeneratingStations.Commands.CreateGeneratingStation;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record CreateGeneratingStationCommand : IRequest<int>
{
    public required string Name { get; init; }
    public required string OwnerIds { get; init; }
    public int VoltageLevelId { get; set; }
    public int LocationId { get; set; }
    public bool IsAc { get; set; } = true;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double InstalledCapacity { get; set; }
    public double MvaCapacity { get; set; }
    public int GeneratingStationClassificationId { get; set; }
    public int GeneratingStationTypeId { get; set; }
    public int FuelId { get; set; }
}

public class CreateGeneratingStationCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateGeneratingStationCommand, int>
{
    public async Task<int> Handle(CreateGeneratingStationCommand request, CancellationToken cancellationToken)
    {
        Location location = await context.Locations.FirstOrDefaultAsync(l => l.Id == request.LocationId, cancellationToken)
                            ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "Location Id is not present in database"
                                                                                }]);
        VoltageLevel voltLvl = await context.VoltageLevels.FirstOrDefaultAsync(l => l.Id == request.VoltageLevelId, cancellationToken)
                            ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "Voltage level Id is not present in database"
                                                                                }]);
        List<Owner> owners = await OwnerUtils.GetOwnersFromIdsAsync(request.OwnerIds, context, cancellationToken);

        string ownersCache = OwnerUtils.DeriveOwnersCache(owners);

        var entity = new GeneratingStation()
        {
            Name = request.Name,
            OwnerNamesCache = ownersCache,
            RegionCache = location.RegionCache,
            VoltageLevelId = request.VoltageLevelId,
            LocationId = request.LocationId,
            IsAc = request.IsAc,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            InstalledCapacity = request.InstalledCapacity,
            MvaCapacity = request.MvaCapacity,
            GeneratingStationTypeId = request.GeneratingStationTypeId,
            GeneratingStationClassificationId = request.GeneratingStationClassificationId,
            FuelId = request.FuelId
        };

        context.GeneratingStations.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        // create substation owner objects
        foreach (Owner owner in owners)
        {
            var substationOwner = new SubstationOwner
            {
                OwnerId = owner.Id,
                SubstationId = entity.Id
            };
            context.SubstationOwners.Add(substationOwner);
        }
        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
