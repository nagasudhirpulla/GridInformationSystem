using App.Common.Interfaces;
using Core.Entities;
using MediatR;

namespace App.GeneratingStationClassifications.Commands.CreateGenStnClassification;

public record CreateGenStnClassificationCommand : IRequest<int>
{
    public required string Classification { get; init; }
}

public class CreateGenStnClassificationCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateGenStnClassificationCommand, int>
{
    public async Task<int> Handle(CreateGenStnClassificationCommand request, CancellationToken cancellationToken)
    {
        var entity = new GeneratingStationClassification() { Classification = request.Classification };

        context.GeneratingStationClassifications.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
