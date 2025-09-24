using App.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infra.Identity;
public class ApplicationUser : IdentityUser, IUser
{
    public required string DisplayName { get; set; }
}
