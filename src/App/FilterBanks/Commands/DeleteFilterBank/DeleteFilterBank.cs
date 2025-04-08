using App.Common.Behaviours;
using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.FilterBanks.Commands.DeleteFilterBank;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record DeleteFilterBankCommand(int Id) : IRequest;

public class DeleteFilterBankCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteFilterBankCommand>
{
    public async Task Handle(DeleteFilterBankCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.FilterBanks
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.FilterBanks.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
    }
}
