using App.BusReactors.Commands.UpdateBusReactor;
using App.BusReactors.Queries.GetBusReactor;
using App.Common.Interfaces;
using App.Elements.Queries.GetElements;
using App.Owners.Queries.GetOwners;
using Core.Entities.Elements;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using App.Common.Security;
using FluentValidation.AspNetCore;
using App.Buses.Queries.GetBuses;

namespace WebApp.Pages.BusReactors;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class EditModel(ILogger<EditModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required UpdateBusReactorCommand BusReactor { get; set; }
    public async Task OnGetAsync(int id)
    {
        var bay = await mediator.Send(new GetBusReactorQuery() { Id = id });
        BusReactor = new UpdateBusReactorCommand()
        {
            Id = bay.Id,
            OwnerIds = string.Join(',', bay.ElementOwners.Select(x => x.OwnerId)),
            ElementNumber = bay.ElementNumber,
            CommissioningDate = bay.CommissioningDate,
            DeCommissioningDate = bay.DeCommissioningDate,
            CommercialOperationDate = bay.CommercialOperationDate,
            IsImportantGridElement = bay.IsImportantGridElement,
            BusId = bay.BusId,
            MvarCapacity = bay.MvarCapacity
        };
        await InitSelectListsAsync();
    }

    private async Task InitSelectListsAsync()
    {
        ViewData["BusId"] = new SelectList(await mediator.Send(new GetBusesQuery()), nameof(Bus.Id), nameof(Bus.ElementNameCache));
        ViewData["OwnerId"] = new MultiSelectList(await mediator.Send(new GetOwnersQuery()), nameof(Owner.Id), nameof(Owner.Name), BusReactor.OwnerIds.Split(','));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new UpdateBusReactorCommandValidator(context);
        var validationResult = await validator.ValidateAsync(BusReactor);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            await InitSelectListsAsync();
            return Page();
        }

        await mediator.Send(BusReactor);
        logger.LogInformation($"Updated BusReactor with {BusReactor.Id}");
        return RedirectToPage("./Index");
    }
}
