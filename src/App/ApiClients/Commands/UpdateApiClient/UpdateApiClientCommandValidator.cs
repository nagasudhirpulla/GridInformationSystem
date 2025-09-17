using App.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.ApiClients.Commands.UpdateApiClient;

public class UpdateApiClientCommandValidator : AbstractValidator<UpdateApiClientCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateApiClientCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.ApiRoleIds)
            .NotEmpty();

        RuleFor(v => v)
            .MustAsync(BeUniqueName)
                .WithMessage("name should be unique")
                .WithErrorCode("Unique");

        RuleFor(v => v)
            .MustAsync(BeUniqueKey)
                .WithMessage("key should be unique")
                .WithErrorCode("Unique");

        RuleFor(v => v.ApiRoleIds)
            .Must(BeValidRoleIds)
                .WithMessage("invalid role Ids provided")
                .WithErrorCode("Invalid");

    }
    public async Task<bool> BeUniqueName(UpdateApiClientCommand cmd, CancellationToken cancellationToken)
    {
        var isNamePresent = await _context.ApiClients
            .AnyAsync(s => (s.Id != cmd.Id) && (s.Name == cmd.Name), cancellationToken);
        return !isNamePresent;
    }

    public async Task<bool> BeUniqueKey(UpdateApiClientCommand cmd, CancellationToken cancellationToken)
    {
        var isKeyPresent = await _context.ApiClients
            .AnyAsync(s => (s.Id != cmd.Id) && (s.Key == cmd.Key), cancellationToken);
        return !isKeyPresent;
    }

    public static bool BeValidRoleIds(string rIds)
    {
        return rIds.Split(',').Any(rId =>
        {
            int roleId = -1;
            try
            {
                roleId = Int32.Parse(rId);
            }
            catch (FormatException)
            {

                return false;
            }
            return true;
        });
    }
}
