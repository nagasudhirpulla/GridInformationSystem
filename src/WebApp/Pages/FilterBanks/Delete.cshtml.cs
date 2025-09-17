using App.Common.Security;
using App.FilterBanks.Commands.DeleteFilterBank;
using App.FilterBanks.Queries.GetFilterBank;
using Core.Entities.Elements;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.FilterBanks;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class DeleteModel(ILogger<CreateModel> logger, IMediator mediator) : PageModel
{
    public required FilterBank FilterBank { get; set; }

    [BindProperty]
    public required DeleteFilterBankCommand DelFilterBankCmd { get; set; }

    public async Task OnGetAsync(int id)
    {
        FilterBank = await mediator.Send(new GetFilterBankQuery() { Id = id });
        DelFilterBankCmd = new DeleteFilterBankCommand(id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await mediator.Send(DelFilterBankCmd);
        logger.LogInformation($"Deleted FilterBank with id {DelFilterBankCmd.Id}");
        return RedirectToPage("./Index");
    }

}
