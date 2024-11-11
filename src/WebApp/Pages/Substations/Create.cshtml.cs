using App.Common.Interfaces;
using App.Common.Security;
using App.Locations.Queries.GetLocations;
using App.Owners.Queries.GetOwners;
using App.Substations.Commands.CreateSubstation;
using App.VoltageLevels.Queries.GetVoltageLevels;
using Core.Entities;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages.Substations;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class CreateModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required CreateSubstationCommand NewSubstation { get; set; }
    public async Task OnGetAsync()
    {
        await InitSelectListsAsync();
    }

    private async Task InitSelectListsAsync()
    {
        ViewData["LocationId"] = new SelectList(await mediator.Send(new GetLocationsQuery()), nameof(Location.Id), nameof(Location.Name));
        ViewData["VoltageLevelId"] = new SelectList(await mediator.Send(new GetVoltageLevelsQuery()), nameof(VoltageLevel.Id), nameof(VoltageLevel.Level));
        ViewData["OwnerId"] = new MultiSelectList(await mediator.Send(new GetOwnersQuery()), nameof(Owner.Id), nameof(Owner.Name), NewSubstation?.OwnerIds.Split(','));

    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new CreateSubstationCommandValidator(context);
        var validationResult = await validator.ValidateAsync(NewSubstation);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            await InitSelectListsAsync();
            return Page();
        }


        var newSubstationId = await mediator.Send(NewSubstation);
        if (newSubstationId > 0)
        {
            logger.LogInformation($"Created Substation with substationId {newSubstationId}");
            return RedirectToPage("./Index");
        }

        await InitSelectListsAsync();
        // If we got this far, something failed, redisplay form
        return Page();
    }
}