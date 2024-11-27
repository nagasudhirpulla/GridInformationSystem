using App.Common.Behaviours;
using App.Common.Interfaces;
using Ardalis.GuardClauses;
using Core.Entities;
using MediatR;

namespace App.Regions.Commands.UpdateRegion;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record UpdateRegionCommand : IRequest
{
    public int Id { get; init; }

    public required string Name { get; init; }
}

public class UpdateRegionCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateRegionCommand>
{
    public async Task Handle(UpdateRegionCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Regions
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        var existingRegionName = entity.Name;

        entity.Name = request.Name;

        await context.SaveChangesAsync(cancellationToken);

        // update region name in cache column of all tables
        await context.ReplaceSubstringInColumn(existingRegionName, request.Name, nameof(Location.RegionCache), cancellationToken);
    }
}
