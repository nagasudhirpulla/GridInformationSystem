using App.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.GeneratingStationTypes.Commands.UpdateGenStationType;

public class UpdateGenStationTypeCommandValidator : AbstractValidator<UpdateGenStationTypeCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateGenStationTypeCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.StationType)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueName)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");
    }

    public async Task<bool> BeUniqueName(UpdateGenStationTypeCommand model, string title, CancellationToken cancellationToken)
    {
        return await _context.GeneratingStationTypes
            .Where(l => l.Id != model.Id)
            .AllAsync(l => l.StationType != title, cancellationToken);
    }

}
