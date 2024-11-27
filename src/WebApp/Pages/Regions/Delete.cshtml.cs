using App.Common.Interfaces;
using App.Common.Security;
using App.Regions.Commands.DeleteRegion;
using App.Regions.Queries.GetRegion;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Regions;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class DeleteModel(ILogger<CreateModel> logger, IMediator mediator) : PageModel
{
    public required Region Region { get; set; }

    [BindProperty]
    public required DeleteRegionCommand DelRegionCmd { get; set; }

    public async Task OnGetAsync(int id)
    {
        Region = await mediator.Send(new GetRegionQuery() { Id = id });
        DelRegionCmd = new DeleteRegionCommand(id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await mediator.Send(DelRegionCmd);
        logger.LogInformation("Deleted Region with id {Id}", DelRegionCmd.Id);
        return RedirectToPage("./Index");
    }

}
