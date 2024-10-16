using App.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.Owners.Commands.CreateOwner;

public class CreateOwnerCommandValidator : AbstractValidator<CreateOwnerCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateOwnerCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueName)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");
    }

    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.Owners
            .AllAsync(l => l.Name != name, cancellationToken);
    }
}