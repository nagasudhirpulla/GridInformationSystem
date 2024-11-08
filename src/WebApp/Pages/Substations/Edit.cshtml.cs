using App.Common.Interfaces;
using App.Common.Security;
using App.Locations.Queries.GetLocations;
using App.Owners.Queries.GetOwners;
using App.Substations.Commands.UpdateSubstation;
using App.Substations.Queries.GetSubstation;
using App.VoltageLevels.Queries.GetVoltageLevels;
using Core.Entities;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages.Substations;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class EditModel(ILogger<EditModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required UpdateSubstationCommand Substation { get; set; }
    public async Task OnGetAsync(int id)
    {
        var substation = await mediator.Send(new GetSubstationQuery() { Id = id });
        Substation = new UpdateSubstationCommand()
        {
            Id = substation.Id,
            VoltageLevelId = substation.VoltageLevelId,
            IsAc = substation.IsAc,
            LocationId = substation.LocationId,
            Latitude = substation.Latitude,
            Longitude = substation.Longitude,
            OwnerIds = string.Join(',', substation.SubstationOwners.Select(x => x.OwnerId))
        };
        await InitSelectListsAsync();
    }

    private async Task InitSelectListsAsync()
    {
        ViewData["LocationId"] = new SelectList(await mediator.Send(new GetLocationsQuery()), nameof(Location.Id), nameof(Location.Name));
        ViewData["VoltageLevelId"] = new SelectList(await mediator.Send(new GetVoltageLevelsQuery()), nameof(VoltageLevel.Id), nameof(VoltageLevel.Level));
        ViewData["OwnerId"] = new MultiSelectList(await mediator.Send(new GetOwnersQuery()), nameof(Owner.Id), nameof(Owner.Name),Substation.OwnerIds.Split(','));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new UpdateSubstationCommandValidator(context);
        var validationResult = await validator.ValidateAsync(Substation);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            await InitSelectListsAsync();
            return Page();
        }

        await mediator.Send(Substation);
        logger.LogInformation($"Updated Substation with {Substation.Id}");
        return RedirectToPage("./Index");
    }
}