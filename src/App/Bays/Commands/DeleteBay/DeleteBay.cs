using App.Common.Behaviours;
using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Bays.Commands.DeleteBay;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record DeleteBayCommand(int Id) : IRequest;

public class DeleteBayCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteBayCommand>
{
    public async Task Handle(DeleteBayCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Bays
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.Bays.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
    }
}