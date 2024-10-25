using App.Common.Interfaces;
using App.Common.Security;
using App.Owners.Commands.UpdateOwner;
using App.Owners.Queries.GetOwner;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Owners;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class EditModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required UpdateOwnerCommand Owner { get; set; }
    public async Task OnGetAsync(int id)
    {
        var region = await mediator.Send(new GetOwnerQuery() { Id = id });
        Owner = new UpdateOwnerCommand() { Id = region.Id, Name = region.Name };
    }


    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new UpdateOwnerCommandValidator(context);
        var validationResult = await validator.ValidateAsync(Owner);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            return Page();
        }

        await mediator.Send(Owner);
        logger.LogInformation($"Updated Owner name to {Owner.Name}");
        return RedirectToPage("./Index");
    }
}