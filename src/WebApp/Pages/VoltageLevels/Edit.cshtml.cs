using App.Common.Interfaces;
using App.Common.Security;
using App.VoltageLevels.Commands.UpdateVoltageLevel;
using App.VoltageLevels.Queries.GetVoltageLevel;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.VoltageLevels;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class EditModel(ILogger<EditModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required UpdateVoltageLevelCommand VoltageLevel { get; set; }
    public async Task OnGetAsync(int id)
    {
        var voltLvl = await mediator.Send(new GetVoltageLevelQuery() { Id = id });
        VoltageLevel = new UpdateVoltageLevelCommand() { Id = voltLvl.Id, Level = voltLvl.Level };
    }


    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new UpdateVoltageLevelCommandValidator(context);
        var validationResult = await validator.ValidateAsync(VoltageLevel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            return Page();
        }

        await mediator.Send(VoltageLevel);
        logger.LogInformation($"Updated VoltageLevel name to {VoltageLevel.Level}");
        return RedirectToPage("./Index");
    }
}