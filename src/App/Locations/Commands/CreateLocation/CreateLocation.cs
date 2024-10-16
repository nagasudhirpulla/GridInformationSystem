using App.Common.Interfaces;
using Core.Entities;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Locations.Commands.CreateLocation;

public record CreateLocationCommand : IRequest<int>
{
    public required string Name { get; init; }
    public string? Alias { get; set; }
    public int StateId { get; set; }

}

public class CreateLocationCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateLocationCommand, int>
{
    public async Task<int> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
    {
        var state = await context.States.Include(s => s.Region)
                    .Where(s => s.Id == request.StateId)
                    .FirstOrDefaultAsync(cancellationToken) ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                    ErrorMessage = "State Id is not present in database"
                                                                }]);
        var regionName = state.Region.Name;
        var entity = new Location()
        {
            Name = request.Name,
            StateId = request.StateId,
            RegionCache = regionName
        };

        context.Locations.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
