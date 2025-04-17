using App.Bays.Commands.UpdateBay;
using App.Bays.Queries.GetBay;
using App.Common.Interfaces;
using App.Common.Security;
using App.Elements.Queries.GetElements;
using App.Owners.Queries.GetOwners;
using Core.Entities;
using Core.Entities.Elements;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages.Bays;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class EditModel(ILogger<EditModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required UpdateBayCommand Bay { get; set; }
    public async Task OnGetAsync(int id)
    {
        var bay = await mediator.Send(new GetBayQuery() { Id = id });
        Bay = new UpdateBayCommand()
        {
            Id = bay.Id,
            Element1Id = bay.Element1Id,
            Element2Id = bay.Element2Id,
            OwnerIds = string.Join(',', bay.ElementOwners.Select(x => x.OwnerId)),
            CommissioningDate = bay.CommissioningDate,
            DeCommissioningDate = bay.DeCommissioningDate,
            CommercialOperationDate = bay.CommercialOperationDate,
            IsImportantGridElement = bay.IsImportantGridElement,
            IsFuture = bay.IsFuture,
            BayType = bay.BayType
        };
        await InitSelectListsAsync();
    }

    private async Task InitSelectListsAsync()
    {
        ViewData["ElementId"] = new SelectList(await mediator.Send(new GetElementsQuery()), nameof(Element.Id), nameof(Element.ElementNameCache));
        ViewData["OwnerId"] = new MultiSelectList(await mediator.Send(new GetOwnersQuery()), nameof(Owner.Id), nameof(Owner.Name), Bay.OwnerIds.Split(','));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new UpdateBayCommandValidator(context);
        var validationResult = await validator.ValidateAsync(Bay);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            await InitSelectListsAsync();
            return Page();
        }

        await mediator.Send(Bay);
        logger.LogInformation($"Updated Bay with {Bay.Id}");
        return RedirectToPage("./Index");
    }
}