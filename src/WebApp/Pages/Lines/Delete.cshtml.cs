using App.Common.Security;
using Core.Entities.Elements;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using App.Lines.Commands.DeleteLine;
using App.Lines.Queries.GetLine;

namespace WebApp.Pages.Lines;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class DeleteModel(ILogger<CreateModel> logger, IMediator mediator) : PageModel
{
    public required Line Line { get; set; }

    [BindProperty]
    public required DeleteLineCommand DelLineCmd { get; set; }

    public async Task OnGetAsync(int id)
    {
        Line = await mediator.Send(new GetLineQuery() { Id = id });
        DelLineCmd = new DeleteLineCommand(id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await mediator.Send(DelLineCmd);
        logger.LogInformation($"Deleted Line with id {DelLineCmd.Id}");
        return RedirectToPage("./Index");
    }

}