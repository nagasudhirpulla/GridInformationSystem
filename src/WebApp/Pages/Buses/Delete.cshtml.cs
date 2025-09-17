using App.Buses.Commands.DeleteBus;
using App.Buses.Queries.GetBus;
using App.Common.Security;
using Core.Entities.Elements;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Buses;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class DeleteModel(ILogger<CreateModel> logger, IMediator mediator) : PageModel
{
    public required Bus Bus { get; set; }

    [BindProperty]
    public required DeleteBusCommand DelBusCmd { get; set; }

    public async Task OnGetAsync(int id)
    {
        Bus = await mediator.Send(new GetBusQuery() { Id = id });
        DelBusCmd = new DeleteBusCommand(id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await mediator.Send(DelBusCmd);
        logger.LogInformation($"Deleted Bus with id {DelBusCmd.Id}");
        return RedirectToPage("./Index");
    }

}