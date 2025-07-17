using App.Common.Interfaces;
using App.Common.Security;
using App.Datasources.Commands.CreateDatasource;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Datasources;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class CreateModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required CreateDatasourceCommand NewDatasource { get; set; }
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new CreateDatasourceCommandValidator(context);
        var validationResult = await validator.ValidateAsync(NewDatasource);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            return Page();
        }


        var newDatasourceId = await mediator.Send(NewDatasource);
        if (newDatasourceId > 0)
        {
            logger.LogInformation($"Created Datasource with name {NewDatasource.Name}");
            return RedirectToPage("./Index");
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }
}