using App.Common.Interfaces;
using App.Common.Security;
using App.ProxyDatasources.Commands.UpdateProxyDatasource;
using App.ProxyDatasources.Queries.GetProxyDatasource;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.ProxyDatasources;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class EditModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required UpdateProxyDatasourceCommand Datasource { get; set; }
    public async Task OnGetAsync(int id)
    {
        var region = await mediator.Send(new GetProxyDatasourceQuery() { Id = id });
        Datasource = new UpdateProxyDatasourceCommand() { Id = region.Id, Name = region.Name, BaseUrl = region.BaseUrl, ApiKey = region.ApiKey };
    }


    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new UpdateProxyDatasourceCommandValidator(context);
        var validationResult = await validator.ValidateAsync(Datasource);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            return Page();
        }

        await mediator.Send(Datasource);
        logger.LogInformation($"Updated Proxy Datasource name to {Datasource.Name}");
        return RedirectToPage("./Index");
    }
}
