using App.Bays.Commands.CreateBay;
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
public class CreateModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required CreateBayCommand NewBay { get; set; }
    public async Task OnGetAsync()
    {
        await InitSelectListsAsync();
    }

    private async Task InitSelectListsAsync()
    {
        ViewData["ElementId"] = new SelectList(await mediator.Send(new GetElementsQuery()), nameof(Element.Id), nameof(Element.ElementNameCache));
        ViewData["OwnerId"] = new MultiSelectList(await mediator.Send(new GetOwnersQuery()), nameof(Owner.Id), nameof(Owner.Name), NewBay?.OwnerIds.Split(","));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new CreateBayCommandValidator(context);
        var validationResult = await validator.ValidateAsync(NewBay);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            await InitSelectListsAsync();
            return Page();
        }


        var newBayId = await mediator.Send(NewBay);
        if (newBayId > 0)
        {
            logger.LogInformation($"Created Bay with Id {newBayId}");
            return RedirectToPage("./Index");
        }

        await InitSelectListsAsync();
        // If we got this far, something failed, redisplay form
        return Page();
    }
}
