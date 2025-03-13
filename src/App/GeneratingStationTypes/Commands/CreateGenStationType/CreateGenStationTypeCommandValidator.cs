using App.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.GeneratingStationTypes.Commands.CreateGenStationType;

public class CreateGenStationTypeCommandValidator : AbstractValidator<CreateGenStationTypeCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateGenStationTypeCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.StationType)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueName)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");
    }

    public async Task<bool> BeUniqueName(string stationType, CancellationToken cancellationToken)
    {
        return await _context.GeneratingStationTypes
            .AllAsync(l => l.StationType != stationType, cancellationToken);
    }
}