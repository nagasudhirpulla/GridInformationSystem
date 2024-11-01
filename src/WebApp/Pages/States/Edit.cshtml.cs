using App.Common.Interfaces;
using App.Common.Security;
using App.Regions.Queries.GetRegions;
using App.States.Commands.UpdateState;
using App.States.Queries.GetState;
using Core.Entities;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages.States;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class EditModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required UpdateStateCommand State { get; set; }
    public async Task OnGetAsync(int id)
    {
        var state = await mediator.Send(new GetStateQuery() { Id = id });
        State = new UpdateStateCommand() { Id = state.Id, RegionId = state.RegionId, Name = state.Name };
        await InitSelectListsAsync();
    }

    private async Task InitSelectListsAsync()
    {
        ViewData["RegionId"] = new SelectList(await mediator.Send(new GetRegionsQuery()), nameof(Region.Id), nameof(Region.Name));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new UpdateStateCommandValidator(context);
        var validationResult = await validator.ValidateAsync(State);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            await InitSelectListsAsync();
            return Page();
        }

        await mediator.Send(State);
        logger.LogInformation($"Updated State name to {State.Name} and region id {State.RegionId}");
        return RedirectToPage("./Index");
    }
}

