using App.Common.Interfaces;
using App.Common.Security;
using App.Owners.Queries.GetOwners;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using App.Lines.Commands.CreateLine;
using Core.Entities.Elements;
using FluentValidation.AspNetCore;
using App.Buses.Queries.GetBuses;

namespace WebApp.Pages.Lines;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class CreateModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required CreateLineCommand NewLine { get; set; }
    public async Task OnGetAsync()
    {
        await InitSelectListsAsync();
    }

    private async Task InitSelectListsAsync()
    {
        ViewData["Bus1Id"] = new SelectList(await mediator.Send(new GetBusesQuery()), nameof(Bus.Id), nameof(Bus.Name));
        ViewData["Bus2Id"] = new SelectList(await mediator.Send(new GetBusesQuery()), nameof(Bus.Id), nameof(Bus.Name));
        ViewData["OwnerId"] = new MultiSelectList(await mediator.Send(new GetOwnersQuery()), nameof(Owner.Id), nameof(Owner.Name), NewLine?.OwnerIds.Split(","));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new CreateLineCommandValidator(context);
        var validationResult = await validator.ValidateAsync(NewLine);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            await InitSelectListsAsync();
            return Page();
        }


        var newLineId = await mediator.Send(NewLine);
        if (newLineId > 0)
        {
            logger.LogInformation($"Created Line with Id {newLineId}");
            return RedirectToPage("./Index");
        }

        await InitSelectListsAsync();
        // If we got this far, something failed, redisplay form
        return Page();
    }
}
