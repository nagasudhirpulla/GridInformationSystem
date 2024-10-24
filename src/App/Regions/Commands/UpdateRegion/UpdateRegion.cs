using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;

namespace App.Regions.Commands.UpdateRegion;

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

        entity.Name = request.Name;

        await context.SaveChangesAsync(cancellationToken);
    }
}
