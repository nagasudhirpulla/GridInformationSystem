using App.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.Datasources.Commands.UpdateDatasource;

public class UpdateDatasourceCommandValidator : AbstractValidator<UpdateDatasourceCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateDatasourceCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueName)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");
    }

    public async Task<bool> BeUniqueName(UpdateDatasourceCommand model, string title, CancellationToken cancellationToken)
    {
        return await _context.Datasources
            .Where(l => l.Id != model.Id)
            .AllAsync(l => l.Name != title, cancellationToken);
    }

}

