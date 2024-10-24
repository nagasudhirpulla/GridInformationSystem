using App.Common.Interfaces;
using App.Common.Security;
using App.VoltageLevels.Commands.CreateVoltageLevel;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.VoltageLevels;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class CreateModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required CreateVoltageLevelCommand NewVoltageLevel { get; set; }
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new CreateVoltageLevelCommandValidator(context);
        var validationResult = await validator.ValidateAsync(NewVoltageLevel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            return Page();
        }


        var newVoltageLevelId = await mediator.Send(NewVoltageLevel);
        if (newVoltageLevelId > 0)
        {
            logger.LogInformation($"Created VoltageLevel with name {NewVoltageLevel.Level}");
            return RedirectToPage("./Index");
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }
}