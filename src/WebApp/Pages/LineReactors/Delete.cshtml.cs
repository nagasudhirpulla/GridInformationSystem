using App.Common.Security;
using App.LineReactors.Commands.DeleteLineReactor;
using App.LineReactors.Queries.GetLineReactor;
using Core.Entities.Elements;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.LineReactors;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class DeleteModel(ILogger<CreateModel> logger, IMediator mediator) : PageModel
{
    public required LineReactor LineReactor { get; set; }

    [BindProperty]
    public required DeleteLineReactorCommand DelLineReactorCmd { get; set; }

    public async Task OnGetAsync(int id)
    {
        LineReactor = await mediator.Send(new GetLineReactorQuery() { Id = id });
        DelLineReactorCmd = new DeleteLineReactorCommand(id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await mediator.Send(DelLineReactorCmd);
        logger.LogInformation($"Deleted LineReactor with id {DelLineReactorCmd.Id}");
        return RedirectToPage("./Index");
    }

}
