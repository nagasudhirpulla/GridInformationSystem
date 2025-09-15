using App.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.ApiRoles.Commands.UpdateApiRole;

public class UpdateApiRoleCommandValidator : AbstractValidator<UpdateApiRoleCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateApiRoleCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.ApiRoleName)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueName)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");
    }

    public async Task<bool> BeUniqueName(UpdateApiRoleCommand model, string title, CancellationToken cancellationToken)
    {
        return await _context.ApiRoles
            .Where(l => l.Id != model.Id)
            .AllAsync(l => l.Name != title, cancellationToken);
    }

}
