using App.Common.Interfaces;
using Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Regions.Commands.CreateRegion;

public record CreateRegionCommand : IRequest<int>
{
    public required string Name { get; init; }
}

public class CreateRegionCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateRegionCommand, int>
{
    public async Task<int> Handle(CreateRegionCommand request, CancellationToken cancellationToken)
    {
        var entity = new Region() { Name = request.Name };

        context.Regions.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
