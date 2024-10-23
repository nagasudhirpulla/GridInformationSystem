using App.Common.Interfaces;
using App.Common.Security;
using App.States.Commands.DeleteState;
using App.States.Commands.UpdateState;
using App.States.Queries.GetState;
using Core.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.States;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class DeleteModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    public required State State { get; set; }

    [BindProperty]
    public required DeleteStateCommand DelStateCmd { get; set; }

    public async Task OnGetAsync(int id)
    {
        State = await mediator.Send(new GetStateQuery() { Id = id });
        DelStateCmd = new DeleteStateCommand(id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await mediator.Send(DelStateCmd);
        logger.LogInformation($"Deleted State with id {DelStateCmd.Id}");
        return RedirectToPage("./Index");
    }

}
