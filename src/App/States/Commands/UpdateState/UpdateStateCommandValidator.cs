using App.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.States.Commands.UpdateState;

public class UpdateStateCommandValidator : AbstractValidator<UpdateStateCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateStateCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueName)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");

        RuleFor(v => v.RegionId)
            .MustAsync(async (int rId, CancellationToken cancellationToken) =>
            {
                return await _context.Regions.AnyAsync(r => r.Id == rId, cancellationToken);
            })
                .WithMessage("'{PropertyName}' does not exist.")
                .WithErrorCode("NotExists"); ;
    }

    public async Task<bool> BeUniqueName(UpdateStateCommand model, string title, CancellationToken cancellationToken)
    {
        return await _context.States
            .Where(l => l.Id != model.Id)
            .AllAsync(l => l.Name != title, cancellationToken);
    }

}