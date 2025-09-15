using App.Common.Interfaces;
using App.Owners.Utils;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.ApiClients.Commands.CreateApiClient;

public class CreateApiClientCommandValidator : AbstractValidator<CreateApiClientCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateApiClientCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.ApiRoleIds)
            .NotEmpty();

        RuleFor(v => v.ApiRoleIds)
            .Must(OwnerUtils.BeValidOwnerIds)
                .WithMessage("invalid Api role Ids provided")
                .WithErrorCode("Invalid");

        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueName)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");

        RuleFor(v => v.Key)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueKey)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");

    }
    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.ApiClients
            .AllAsync(l => l.Name != name, cancellationToken);
    }

    public async Task<bool> BeUniqueKey(string key, CancellationToken cancellationToken)
    {
        return await _context.ApiClients
            .AllAsync(l => l.Key != key, cancellationToken);
    }
}