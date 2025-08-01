using App.Common.Interfaces;
using App.Common.Security;
using App.SubFilterBanks.Commands.UpdateSubFilterBank;
using App.SubFilterBanks.Queries.GetSubFilterBank;
using App.Owners.Queries.GetOwners;
using Core.Entities;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using App.FilterBanks.Queries.GetFilterBanks;
using Core.Entities.Elements;

namespace WebApp.Pages.SubFilterBanks;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class EditModel(ILogger<EditModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required UpdateSubFilterBankCommand SubFilterBank { get; set; }
    public async Task OnGetAsync(int id)
    {
        var filterBank = await mediator.Send(new GetSubFilterBankQuery() { Id = id });
        SubFilterBank = new UpdateSubFilterBankCommand()
        {
            Id = filterBank.Id,
            FilterBankId = filterBank.Substation1Id,
            OwnerIds = string.Join(',', filterBank.ElementOwners.Select(x => x.OwnerId)),
            SubFilterTag = filterBank.ElementNumber,
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
        ViewData["FilterBankId"] = new SelectList(await mediator.Send(new GetFilterBanksQuery()), nameof(FilterBank.Id), nameof(FilterBank.Name));
        ViewData["OwnerId"] = new MultiSelectList(await mediator.Send(new GetOwnersQuery()), nameof(Owner.Id), nameof(Owner.Name), SubFilterBank.OwnerIds.Split(','));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new UpdateSubFilterBankCommandValidator(context);
        var validationResult = await validator.ValidateAsync(SubFilterBank);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            await InitSelectListsAsync();
            return Page();
        }

        await mediator.Send(SubFilterBank);
        logger.LogInformation($"Updated SubFilterBank with {SubFilterBank.Id}");
        return RedirectToPage("./Index");
    }
}