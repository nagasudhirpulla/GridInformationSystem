using App.Common.Interfaces;
using App.Common.Security;
using App.FilterBanks.Queries.GetFilterBanks;
using App.Owners.Queries.GetOwners;
using App.SubFilterBanks.Commands.CreateSubFilterBank;
using Core.Entities;
using Core.Entities.Elements;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages.SubFilterBanks;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class CreateModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required CreateSubFilterBankCommand NewSubFilterBank { get; set; }
    public async Task OnGetAsync()
    {
        await InitSelectListsAsync();
    }

    private async Task InitSelectListsAsync()
    {
        ViewData["FilterBankId"] = new SelectList(await mediator.Send(new GetFilterBanksQuery()), nameof(FilterBank.Id), nameof(FilterBank.Name));
        ViewData["OwnerId"] = new MultiSelectList(await mediator.Send(new GetOwnersQuery()), nameof(Owner.Id), nameof(Owner.Name), NewSubFilterBank?.OwnerIds.Split(","));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new CreateSubFilterBankCommandValidator(context);
        var validationResult = await validator.ValidateAsync(NewSubFilterBank);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            await InitSelectListsAsync();
            return Page();
        }


        var newSubFilterBankId = await mediator.Send(NewSubFilterBank);
        if (newSubFilterBankId > 0)
        {
            logger.LogInformation($"Created SubFilterBank with Id {newSubFilterBankId}");
            return RedirectToPage("./Index");
        }

        await InitSelectListsAsync();
        // If we got this far, something failed, redisplay form
        return Page();
    }
}
