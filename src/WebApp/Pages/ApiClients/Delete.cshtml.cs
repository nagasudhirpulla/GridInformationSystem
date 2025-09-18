using App.ApiClients.Commands.DeleteApiClient;
using App.ApiClients.Queries.GetApiClient;
using App.Common.Security;
using Core.Entities.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.ApiClients;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class DeleteModel(ILogger<CreateModel> logger, IMediator mediator) : PageModel
{
    public required ApiClient ApiClient { get; set; }

    [BindProperty]
    public required DeleteApiClientCommand DelApiClientCmd { get; set; }

    public async Task OnGetAsync(int id)
    {
        ApiClient = await mediator.Send(new GetApiClientQuery() { Id = id });
        DelApiClientCmd = new DeleteApiClientCommand(id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await mediator.Send(DelApiClientCmd);
        logger.LogInformation($"Deleted ApiClient with id {DelApiClientCmd.Id}");
        return RedirectToPage("./Index");
    }

}
