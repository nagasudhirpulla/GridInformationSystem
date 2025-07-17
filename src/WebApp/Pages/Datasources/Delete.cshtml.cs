using App.Common.Security;
using App.Datasources.Commands.DeleteDatasource;
using App.Datasources.Queries.GetDatasource;
using Core.Entities.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Datasources;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class DeleteModel(ILogger<CreateModel> logger, IMediator mediator) : PageModel
{
    public required Datasource Datasource { get; set; }

    [BindProperty]
    public required DeleteDatasourceCommand DelDatasourceCmd { get; set; }

    public async Task OnGetAsync(int id)
    {
        Datasource = await mediator.Send(new GetDatasourceQuery() { Id = id });
        DelDatasourceCmd = new DeleteDatasourceCommand(id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await mediator.Send(DelDatasourceCmd);
        logger.LogInformation("Deleted Datasource with id {Id}", DelDatasourceCmd.Id);
        return RedirectToPage("./Index");
    }

}