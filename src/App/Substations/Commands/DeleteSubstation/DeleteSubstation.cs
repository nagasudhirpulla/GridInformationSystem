using App.Common.Behaviours;
using App.Common.Interfaces;
using App.Substations.Utils;
using Ardalis.GuardClauses;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Substations.Commands.DeleteSubstation;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record DeleteSubstationCommand(int Id) : IRequest;

public class DeleteSubstationCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteSubstationCommand>
{
    public async Task Handle(DeleteSubstationCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Substations
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        bool isElementsConnected = await SubstationUtils.IsElementsConnected(request.Id, context, cancellationToken);
        if (isElementsConnected)
        {
            throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "Can't delete substation when there are connected elements"
                                                                                }]);
        }

        context.Substations.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
    }
}