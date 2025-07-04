using App.Common.Security;
using App.SubFilterBanks.Commands.DeleteSubFilterBank;
using App.SubFilterBanks.Queries.GetSubFilterBank;
using Core.Entities.Elements;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.SubFilterBanks;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class DeleteModel(ILogger<CreateModel> logger, IMediator mediator) : PageModel
{
    public required SubFilterBank SubFilterBank { get; set; }

    [BindProperty]
    public required DeleteSubFilterBankCommand DelSubFilterBankCmd { get; set; }

    public async Task OnGetAsync(int id)
    {
        SubFilterBank = await mediator.Send(new GetSubFilterBankQuery() { Id = id });
        DelSubFilterBankCmd = new DeleteSubFilterBankCommand(id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await mediator.Send(DelSubFilterBankCmd);
        logger.LogInformation($"Deleted SubFilterBank with id {DelSubFilterBankCmd.Id}");
        return RedirectToPage("./Index");
    }

}