using App.Common.Security;
using App.Regions.Queries.GetRegions;
using App.States.Commands.CreateState;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

// TODO complete this - https://github.com/nagasudhirpulla/open_shift_scheduler/blob/master/src/OSS.Web/Pages/Users/Create.cshtml.cs

namespace WebApp.Pages.States
{
    [Authorize(Roles = Core.Constants.Roles.Administrator)]
    public class CreateModel(ILogger<CreateModel> logger, IMediator mediator) : PageModel
    {
        [BindProperty]
        public required CreateStateCommand NewState { get; set; }
        public async Task OnGetAsync()
        {
            await InitSelectListsAsync();
        }

        private async Task InitSelectListsAsync()
        {
            ViewData["RegionId"] = new SelectList(await mediator.Send(new GetRegionsQuery()), "Id", "Name");
        }
    }
}
