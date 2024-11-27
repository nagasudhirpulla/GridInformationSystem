using App.Common.Interfaces;
using App.Common.Security;
using App.Locations.Commands.DeleteLocation;
using App.Locations.Queries.GetLocation;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Locations;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class DeleteModel(ILogger<CreateModel> logger, IMediator mediator) : PageModel
{
    public required Location Location { get; set; }

    [BindProperty]
    public required DeleteLocationCommand DelLocationCmd { get; set; }

    public async Task OnGetAsync(int id)
    {
        Location = await mediator.Send(new GetLocationQuery() { Id = id });
        DelLocationCmd = new DeleteLocationCommand(id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await mediator.Send(DelLocationCmd);
        logger.LogInformation("Deleted Location with id {Id}", DelLocationCmd.Id);
        return RedirectToPage("./Index");
    }

}

