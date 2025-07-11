using App.GeneratingUnits.Commands.UpdateGeneratingUnit;
using App.GeneratingUnits.Queries.GetGeneratingUnit;
using App.Common.Interfaces;
using App.Common.Security;
using App.Owners.Queries.GetOwners;
using Core.Entities;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using App.GeneratingStations.Queries.GetGeneratingStations;

namespace WebApp.Pages.GeneratingUnits;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class EditModel(ILogger<EditModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required UpdateGeneratingUnitCommand GeneratingUnit { get; set; }
    public async Task OnGetAsync(int id)
    {
        var genUnit = await mediator.Send(new GetGeneratingUnitQuery() { Id = id });
        GeneratingUnit = new UpdateGeneratingUnitCommand()
        {
            Id = genUnit.Id,
            GeneratingStationId = genUnit.Substation1Id,
            OwnerIds = string.Join(',', genUnit.ElementOwners.Select(x => x.OwnerId)),
            ElementNumber = genUnit.ElementNumber,
            CommissioningDate = genUnit.CommissioningDate,
            DeCommissioningDate = genUnit.DeCommissioningDate,
            CommercialOperationDate = genUnit.CommercialOperationDate,
            IsImportantGridElement = genUnit.IsImportantGridElement,
            InstalledCapacity = genUnit.Capacity,
            GeneratingVoltage = genUnit.GeneratingVoltage
        };
        await InitSelectListsAsync();
    }

    private async Task InitSelectListsAsync()
    {
        ViewData["SubstationId"] = new SelectList(await mediator.Send(new GetGeneratingStationsQuery()), nameof(GeneratingStation.Id), nameof(GeneratingStation.Name));
        ViewData["OwnerId"] = new MultiSelectList(await mediator.Send(new GetOwnersQuery()), nameof(Owner.Id), nameof(Owner.Name), GeneratingUnit.OwnerIds.Split(','));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new UpdateGeneratingUnitCommandValidator(context);
        var validationResult = await validator.ValidateAsync(GeneratingUnit);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            await InitSelectListsAsync();
            return Page();
        }

        await mediator.Send(GeneratingUnit);
        logger.LogInformation($"Updated GeneratingUnit with {GeneratingUnit.Id}");
        return RedirectToPage("./Index");
    }
}
