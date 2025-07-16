using App.Common.Interfaces;
using FluentValidation;

namespace App.Measurements.Commands.CreateMeasurement;

public class CreateMeasurementCommandValidator : AbstractValidator<CreateMeasurementCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateMeasurementCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.HistorianPntId)
            .NotEmpty()
            .MaximumLength(200);
    }
}