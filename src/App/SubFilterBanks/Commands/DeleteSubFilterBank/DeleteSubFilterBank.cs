using App.Common.Behaviours;
using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.SubFilterBanks.Commands.DeleteSubFilterBank;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record DeleteSubFilterBankCommand(int Id) : IRequest;

public class DeleteSubFilterBankCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteSubFilterBankCommand>
{
    public async Task Handle(DeleteSubFilterBankCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.SubFilterBanks
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.SubFilterBanks.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
    }
}

