using App.Common.Security;
using App.ProxyDatasources.Commands.DeleteProxyDatasource;
using App.ProxyDatasources.Queries.GetProxyDatasource;
using Core.Entities.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.ProxyDatasources;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class DeleteModel(ILogger<CreateModel> logger, IMediator mediator) : PageModel
{
    public required ProxyDatasource Datasource { get; set; }

    [BindProperty]
    public required DeleteProxyDatasourceCommand DelDatasourceCmd { get; set; }

    public async Task OnGetAsync(int id)
    {
        Datasource = await mediator.Send(new GetProxyDatasourceQuery() { Id = id });
        DelDatasourceCmd = new DeleteProxyDatasourceCommand(id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await mediator.Send(DelDatasourceCmd);
        logger.LogInformation("Deleted Proxy Datasource with id {Id}", DelDatasourceCmd.Id);
        return RedirectToPage("./Index");
    }

}
