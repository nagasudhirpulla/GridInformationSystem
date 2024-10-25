using App.Common.Interfaces;
using App.Substations.Utils;
using Core.Entities;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Substations.Commands.CreateSubstation;

public record CreateSubstationCommand : IRequest<int>
{
    public required string OwnerIds { get; init; }
    public int VoltageLevelId { get; set; }
    public int LocationId { get; set; }
    public bool IsAc { get; set; } = true;
    public double Latitude { get; set; }
    public double Longitude { get; set; }

}

public class CreateSubstationCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateSubstationCommand, int>
{
    public async Task<int> Handle(CreateSubstationCommand request, CancellationToken cancellationToken)
    {
        Location location = await context.Locations.FirstOrDefaultAsync(l => l.Id == request.LocationId, cancellationToken)
                            ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "Location Id is not present in database"
                                                                                }]);
        VoltageLevel voltLvl = await context.VoltageLevels.FirstOrDefaultAsync(l => l.Id == request.VoltageLevelId, cancellationToken)
                            ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "Voltage level Id is not present in database"
                                                                                }]);
        string substationName = SubstationUtils.DeriveSubstationName(voltLvl.Level, location.Name);

        // TODO move this command validation
        var ownerIds = request.OwnerIds.Split(",").Select(oId =>
        {
            int ownerId = -1;
            try
            {
                ownerId = Int32.Parse(oId);
            }
            catch (FormatException)
            {

                throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = $"Owner Id {oId} is not valid"
                                                                                }]);
            }
            return ownerId;
        });

        var entity = new Substation()
        {
            VoltageLevelId = request.VoltageLevelId,
            LocationId = request.LocationId,
            IsAc = request.IsAc,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            RegionCache = location.RegionCache,
            NameCache = substationName,
            OwnerIdsCache = request.OwnerIds
        };

        context.Substations.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
