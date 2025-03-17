using App.Common.Behaviours;
using App.Common.Interfaces;
using App.Substations.Utils;
using Ardalis.GuardClauses;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.GeneratingStations.Commands.DeleteGeneratingStation;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record DeleteGeneratingStationCommand(int Id) : IRequest;

public class DeleteGeneratingStationCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteGeneratingStationCommand>
{
    public async Task Handle(DeleteGeneratingStationCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.GeneratingStations
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        bool isElementsConnected = await SubstationUtils.IsElementsConnected(request.Id, context, cancellationToken);
        if (isElementsConnected)
        {
            throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "Can't delete generating station when there are connected elements"
                                                                                }]);
        }

        context.GeneratingStations.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
    }
}
