using App.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.States.Commands.CreateState;

public class CreateStateCommandValidator : AbstractValidator<CreateStateCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateStateCommandValidator(IApplicationDbContext context)
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
        return await _context.States
            .AllAsync(l => l.Name != name, cancellationToken);
    }
}