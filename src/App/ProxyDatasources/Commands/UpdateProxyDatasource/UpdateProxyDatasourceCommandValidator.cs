using App.Common.Interfaces;
using App.ProxyDatasources.Commands.CreateProxyDatasource;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.ProxyDatasources.Commands.UpdateProxyDatasource;

public class UpdateProxyDatasourceCommandValidator : AbstractValidator<UpdateProxyDatasourceCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateProxyDatasourceCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueName)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");

        RuleFor(v => v.BaseUrl)
            .NotEmpty()
            .MaximumLength(200)
            .Must(CreateProxyDatasourceCommandValidator.BeValidUrl)
                .WithMessage("'{PropertyName}' must be valid URL.")
                .WithErrorCode("Invalid");
    }

    public async Task<bool> BeUniqueName(UpdateProxyDatasourceCommand model, string title, CancellationToken cancellationToken)
    {
        return await _context.ProxyDatasources
            .Where(l => l.Id != model.Id)
            .AllAsync(l => l.Name != title, cancellationToken);
    }

}

