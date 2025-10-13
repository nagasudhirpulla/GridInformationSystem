﻿using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;

namespace App.States.Commands.UpdateState;

public record UpdateStateCommand : IRequest
{
    public int Id { get; init; }

    public required string Name { get; init; }
    public int RegionId { get; init; }
}

public class UpdateStateCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateStateCommand>
{
    public async Task Handle(UpdateStateCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.States
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        // update entity attributes
        entity.Name = request.Name;
        entity.RegionId = request.RegionId;
        // TODO update region cache of elements in this state

        await context.SaveChangesAsync(cancellationToken);
    }
}
