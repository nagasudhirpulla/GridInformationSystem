using App.Common.Interfaces;
using Ardalis.GuardClauses;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Locations.Commands.UpdateLocation;

public record UpdateLocationCommand : IRequest
{
    public int Id { get; init; }

    public required string Name { get; init; }
    public string? Alias { get; set; }
    public int StateId { get; set; }
}

public class UpdateLocationCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateLocationCommand>
{
    public async Task Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Locations
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        bool isLocationNameChanged = entity.Name != request.Name;
        bool isLocationStateChanged = entity.StateId != request.StateId;
        if (isLocationNameChanged)
        {
            entity.Name = request.Name;
        }
        entity.Alias = request.Alias;
        if (isLocationStateChanged)
        {
            var state = await context.States.Include(s => s.Region)
                    .Where(s => s.Id == request.StateId)
                    .FirstOrDefaultAsync(cancellationToken) ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                    ErrorMessage = "State Id is not present in database"
                                                                }]);
            var regionName = state.Region.Name;
            // update entity attributes
            entity.StateId = request.StateId;
            entity.RegionCache = regionName;

        }
        await context.SaveChangesAsync(cancellationToken);
    }
}
