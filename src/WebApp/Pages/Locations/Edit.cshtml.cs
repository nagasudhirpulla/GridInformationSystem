using App.Common.Interfaces;
using App.Common.Security;
using App.Locations.Commands.UpdateLocation;
using App.Locations.Queries.GetLocation;
using App.States.Queries.GetStates;
using Core.Entities;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages.Locations;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class EditModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required UpdateLocationCommand Location { get; set; }
    public async Task OnGetAsync(int id)
    {
        var location = await mediator.Send(new GetLocationQuery() { Id = id });
        Location = new UpdateLocationCommand() { Id = location.Id, StateId = location.StateId, Name = location.Name, Alias = location.Alias };
        await InitSelectListsAsync();
    }

    private async Task InitSelectListsAsync()
    {
        ViewData["StateId"] = new SelectList(await mediator.Send(new GetStatesQuery()), nameof(State.Id), nameof(State.Name));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new UpdateLocationCommandValidator(context);
        var validationResult = await validator.ValidateAsync(Location);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            await InitSelectListsAsync();
            return Page();
        }

        await mediator.Send(Location);
        logger.LogInformation($"Updated Location name to {Location.Name} and state id {Location.StateId}");
        return RedirectToPage("./Index");
    }
}

