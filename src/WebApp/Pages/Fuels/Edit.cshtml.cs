using App.Common.Interfaces;
using App.Common.Security;
using App.Fuels.Commands.UpdateFuel;
using App.Fuels.Queries.GetFuel;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Fuels;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class EditModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required UpdateFuelCommand Fuel { get; set; }
    public async Task OnGetAsync(int id)
    {
        var region = await mediator.Send(new GetFuelQuery() { Id = id });
        Fuel = new UpdateFuelCommand() { Id = region.Id, FuelName = region.Name };
    }


    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new UpdateFuelCommandValidator(context);
        var validationResult = await validator.ValidateAsync(Fuel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            return Page();
        }

        await mediator.Send(Fuel);
        logger.LogInformation($"Updated Fuel name to {Fuel.FuelName}");
        return RedirectToPage("./Index");
    }
}
