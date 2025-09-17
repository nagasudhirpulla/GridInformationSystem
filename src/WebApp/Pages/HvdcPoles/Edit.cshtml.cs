using App.Common.Interfaces;
using App.Common.Security;
using App.HvdcPoles.Commands.UpdateHvdcPole;
using App.HvdcPoles.Queries.GetHvdcPole;
using App.Owners.Queries.GetOwners;
using App.Substations.Queries.GetSubstations;
using Core.Entities;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages.HvdcPoles;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class EditModel(ILogger<EditModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required UpdateHvdcPoleCommand HvdcPole { get; set; }
    public async Task OnGetAsync(int id)
    {
        var bus = await mediator.Send(new GetHvdcPoleQuery() { Id = id });
        HvdcPole = new UpdateHvdcPoleCommand()
        {
            Id = bus.Id,
            SubstationId = bus.Substation1Id,
            OwnerIds = string.Join(',', bus.ElementOwners.Select(x => x.OwnerId)),
            ElementNumber = bus.ElementNumber,
            CommissioningDate = bus.CommissioningDate,
            DeCommissioningDate = bus.DeCommissioningDate,
            CommercialOperationDate = bus.CommercialOperationDate,
            IsImportantGridElement = bus.IsImportantGridElement,
            PoleType = bus.PoleType
        };
        await InitSelectListsAsync();
    }

    private async Task InitSelectListsAsync()
    {
        ViewData["SubstationId"] = new SelectList(await mediator.Send(new GetSubstationsQuery()), nameof(Substation.Id), nameof(Substation.Name));
        ViewData["OwnerId"] = new MultiSelectList(await mediator.Send(new GetOwnersQuery()), nameof(Owner.Id), nameof(Owner.Name), HvdcPole.OwnerIds.Split(','));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new UpdateHvdcPoleCommandValidator(context);
        var validationResult = await validator.ValidateAsync(HvdcPole);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            await InitSelectListsAsync();
            return Page();
        }

        await mediator.Send(HvdcPole);
        logger.LogInformation($"Updated HvdcPole with {HvdcPole.Id}");
        return RedirectToPage("./Index");
    }
}
