using App.FilterBanks.Commands.UpdateFilterBank;
using App.FilterBanks.Queries.GetFilterBank;
using App.Common.Interfaces;
using App.Common.Security;
using App.Owners.Queries.GetOwners;
using App.Substations.Queries.GetSubstations;
using Core.Entities;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages.FilterBanks;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class EditModel(ILogger<EditModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required UpdateFilterBankCommand FilterBank { get; set; }
    public async Task OnGetAsync(int id)
    {
        var filterBank = await mediator.Send(new GetFilterBankQuery() { Id = id });
        FilterBank = new UpdateFilterBankCommand()
        {
            Id = filterBank.Id,
            SubstationId = filterBank.Substation1Id,
            OwnerIds = string.Join(',', filterBank.ElementOwners.Select(x => x.OwnerId)),
            ElementNumber = filterBank.ElementNumber,
            CommissioningDate = filterBank.CommissioningDate,
            DeCommissioningDate = filterBank.DeCommissioningDate,
            CommercialOperationDate = filterBank.CommercialOperationDate,
            IsImportantGridElement = filterBank.IsImportantGridElement,
            Mvar = filterBank.Mvar,
            IsSwitchable = filterBank.IsSwitchable
        };
        await InitSelectListsAsync();
    }

    private async Task InitSelectListsAsync()
    {
        ViewData["SubstationId"] = new SelectList(await mediator.Send(new GetSubstationsQuery()), nameof(Substation.Id), nameof(Substation.Name));
        ViewData["OwnerId"] = new MultiSelectList(await mediator.Send(new GetOwnersQuery()), nameof(Owner.Id), nameof(Owner.Name), FilterBank.OwnerIds.Split(','));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new UpdateFilterBankCommandValidator(context);
        var validationResult = await validator.ValidateAsync(FilterBank);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            await InitSelectListsAsync();
            return Page();
        }

        await mediator.Send(FilterBank);
        logger.LogInformation($"Updated FilterBank with {FilterBank.Id}");
        return RedirectToPage("./Index");
    }
}
