using App.Common.Interfaces;
using App.Common.Security;
using App.Locations.Commands.CreateLocation;
using App.States.Queries.GetStates;
using Core.Entities;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages.Locations;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class CreateModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required CreateLocationCommand NewLocation { get; set; }
    public async Task OnGetAsync()
    {
        await InitSelectListsAsync();
    }

    private async Task InitSelectListsAsync()
    {
        ViewData["StateId"] = new SelectList(await mediator.Send(new GetStatesQuery()), nameof(State.Id), nameof(State.Name));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new CreateLocationCommandValidator(context);
        var validationResult = await validator.ValidateAsync(NewLocation);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            await InitSelectListsAsync();
            return Page();
        }


        var newLocationId = await mediator.Send(NewLocation);
        if (newLocationId > 0)
        {
            logger.LogInformation($"Created Location with name {NewLocation.Name}");
            return RedirectToPage("./Index");
        }

        await InitSelectListsAsync();
        // If we got this far, something failed, redisplay form
        return Page();
    }
}
