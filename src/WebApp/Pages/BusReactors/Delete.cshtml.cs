using App.BusReactors.Commands.DeleteBusReactor;
using App.BusReactors.Queries.GetBusReactor;
using App.Common.Security;
using Core.Entities.Elements;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.BusReactors;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class DeleteModel(ILogger<CreateModel> logger, IMediator mediator) : PageModel
{
    public required BusReactor BusReactor { get; set; }

    [BindProperty]
    public required DeleteBusReactorCommand DelBusReactorCmd { get; set; }

    public async Task OnGetAsync(int id)
    {
        BusReactor = await mediator.Send(new GetBusReactorQuery() { Id = id });
        DelBusReactorCmd = new DeleteBusReactorCommand(id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await mediator.Send(DelBusReactorCmd);
        logger.LogInformation($"Deleted BusReactor with id {DelBusReactorCmd.Id}");
        return RedirectToPage("./Index");
    }

}
