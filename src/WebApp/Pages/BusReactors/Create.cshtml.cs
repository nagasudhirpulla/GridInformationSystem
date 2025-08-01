using App.Common.Interfaces;
using App.Owners.Queries.GetOwners;
using Core.Entities.Elements;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using App.Common.Security;
using FluentValidation.AspNetCore;
using App.BusReactors.Commands.CreateBusReactor;
using App.Buses.Queries.GetBuses;

namespace WebApp.Pages.BusReactors;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class CreateModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required CreateBusReactorCommand NewBusReactor { get; set; }
    public async Task OnGetAsync()
    {
        await InitSelectListsAsync();
    }

    private async Task InitSelectListsAsync()
    {
        ViewData["BusId"] = new SelectList(await mediator.Send(new GetBusesQuery()), nameof(Bus.Id), nameof(Bus.Name));
        ViewData["OwnerId"] = new MultiSelectList(await mediator.Send(new GetOwnersQuery()), nameof(Owner.Id), nameof(Owner.Name), NewBusReactor?.OwnerIds.Split(","));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new CreateBusReactorCommandValidator(context);
        var validationResult = await validator.ValidateAsync(NewBusReactor);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            await InitSelectListsAsync();
            return Page();
        }


        var newBusReactorId = await mediator.Send(NewBusReactor);
        if (newBusReactorId > 0)
        {
            logger.LogInformation($"Created BusReactor with Id {newBusReactorId}");
            return RedirectToPage("./Index");
        }

        await InitSelectListsAsync();
        // If we got this far, something failed, redisplay form
        return Page();
    }
}

