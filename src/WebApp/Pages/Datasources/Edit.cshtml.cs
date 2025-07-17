using App.Common.Interfaces;
using App.Common.Security;
using App.Datasources.Commands.UpdateDatasource;
using App.Datasources.Queries.GetDatasource;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Datasources;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class EditModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required UpdateDatasourceCommand Datasource { get; set; }
    public async Task OnGetAsync(int id)
    {
        var region = await mediator.Send(new GetDatasourceQuery() { Id = id });
        Datasource = new UpdateDatasourceCommand() { Id = region.Id, Name = region.Name };
    }


    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new UpdateDatasourceCommandValidator(context);
        var validationResult = await validator.ValidateAsync(Datasource);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            return Page();
        }

        await mediator.Send(Datasource);
        logger.LogInformation($"Updated Datasource name to {Datasource.Name}");
        return RedirectToPage("./Index");
    }
}
