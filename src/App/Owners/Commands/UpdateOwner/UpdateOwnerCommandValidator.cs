using App.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.Owners.Commands.UpdateOwner;

public class UpdateOwnerCommandValidator : AbstractValidator<UpdateOwnerCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateOwnerCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueName)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");
    }

    public async Task<bool> BeUniqueName(UpdateOwnerCommand model, string title, CancellationToken cancellationToken)
    {
        return await _context.Owners
            .Where(l => l.Id != model.Id)
            .AllAsync(l => l.Name != title, cancellationToken);
    }

}
