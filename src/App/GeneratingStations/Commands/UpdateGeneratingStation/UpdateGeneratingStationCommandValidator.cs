using App.Common.Interfaces;
using App.Owners.Utils;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.GeneratingStations.Commands.UpdateGeneratingStation;

public class UpdateGeneratingStationCommandValidator : AbstractValidator<UpdateGeneratingStationCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateGeneratingStationCommandValidator(IApplicationDbContext context)
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
    public async Task<bool> BeUniqueName(UpdateGeneratingStationCommand cmd, CancellationToken cancellationToken)
    {
        var isVoltLocPresent = await _context.GeneratingStations
            .AnyAsync(s => (s.Id != cmd.Id) && (s.Name == cmd.Name), cancellationToken);
        return !isVoltLocPresent;
    }
}
