using Microsoft.AspNetCore.Identity;

namespace Infra.Identity;
public class ApplicationUser : IdentityUser
{
    public required string DisplayName { get; set; }
}
