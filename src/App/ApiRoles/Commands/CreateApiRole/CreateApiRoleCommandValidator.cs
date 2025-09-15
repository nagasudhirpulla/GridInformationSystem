using App.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.ApiRoles.Commands.CreateApiRole;

public class CreateApiRoleCommandValidator : AbstractValidator<CreateApiRoleCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateApiRoleCommandValidator(IApplicationDbContext context)
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
        return await _context.ApiRoles
            .AllAsync(l => l.Name != name, cancellationToken);
    }
}
