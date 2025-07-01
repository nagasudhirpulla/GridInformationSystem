using App.Common.Security;
using App.HvdcLines.Commands.DeleteHvdcLine;
using App.HvdcLines.Queries.GetHvdcLine;
using Core.Entities.Elements;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.HvdcLines;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class DeleteModel(ILogger<CreateModel> logger, IMediator mediator) : PageModel
{
    public required HvdcLine HvdcLine { get; set; }

    [BindProperty]
    public required DeleteHvdcLineCommand DelHvdcLineCmd { get; set; }

    public async Task OnGetAsync(int id)
    {
        HvdcLine = await mediator.Send(new GetHvdcLineQuery() { Id = id });
        DelHvdcLineCmd = new DeleteHvdcLineCommand(id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await mediator.Send(DelHvdcLineCmd);
        logger.LogInformation($"Deleted HvdcLine with id {DelHvdcLineCmd.Id}");
        return RedirectToPage("./Index");
    }

}
