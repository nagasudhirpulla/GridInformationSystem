using App.Common.Behaviours;
using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.ApiClients.Commands.DeleteApiClient;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record DeleteApiClientCommand(int Id) : IRequest;

public class DeleteApiClientCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteApiClientCommand>
{
    public async Task Handle(DeleteApiClientCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.ApiClients
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.ApiClients.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
    }
}