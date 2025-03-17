using App.Common.Interfaces;
using App.Owners.Utils;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.GeneratingStations.Commands.CreateGeneratingStation;

public class CreateGenStationCommandValidator : AbstractValidator<CreateGeneratingStationCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateGenStationCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.OwnerIds)
            .NotEmpty();

        RuleFor(v => v)
            .MustAsync(BeUniqueName)
                .WithMessage("The generating station name should be unique")
                .WithErrorCode("Unique");

        RuleFor(v => v.OwnerIds)
            .Must(OwnerUtils.BeValidOwnerIds)
                .WithMessage("invalid owner Ids provided")
                .WithErrorCode("Unique");

        RuleFor(v => v.MvaCapacity)
            .GreaterThanOrEqualTo(0);

        RuleFor(v => v.Installedcapacity)
            .GreaterThanOrEqualTo(0);

    }
    public async Task<bool> BeUniqueName(CreateGeneratingStationCommand cmd, CancellationToken cancellationToken)
    {
        return !await _context.GeneratingStations
            .AnyAsync(l => (l.Name == cmd.Name), cancellationToken);
    }
}
