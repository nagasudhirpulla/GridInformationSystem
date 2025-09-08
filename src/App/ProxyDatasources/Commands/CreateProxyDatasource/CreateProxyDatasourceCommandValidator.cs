using App.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using NJsonSchema;

namespace App.ProxyDatasources.Commands.CreateProxyDatasource;

public class CreateProxyDatasourceCommandValidator : AbstractValidator<CreateProxyDatasourceCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateProxyDatasourceCommandValidator(IApplicationDbContext context)
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
            .Must(BeValidUrl)
                .WithMessage("'{PropertyName}' must be valid URL.")
                .WithErrorCode("Invalid");
        RuleFor(v => v.PayloadSchema)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeValidJsonSchema)
                .WithMessage("'{PropertyName}' should be valid JSON Schema.")
                .WithErrorCode("Invalid");
    }

    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.ProxyDatasources
            .AllAsync(l => l.Name != name, cancellationToken);
    }

    public static bool BeValidUrl(string url)
    {
        string strRegex = @"((http|https)://)(www.)?" +
          "[a-zA-Z0-9@:%._\\+~#?&//=]" +
          "{2,256}\\.[a-z]" +
          "{2,6}\\b([-a-zA-Z0-9@:%" +
          "._\\+~#?&//=]*)";
        Regex re = new(strRegex);
        if (re.IsMatch(url))
            return true;
        else
            return false;
    }


    public static async Task<bool> BeValidJsonSchema(string schemaStr, CancellationToken cancellationToken)
    {
        try
        {
            _ = await JsonSchema.FromJsonAsync(schemaStr, cancellationToken);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}