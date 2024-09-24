using Microsoft.AspNetCore.Identity;

namespace Core.Entities.Common;

public class ApplicationUser : IdentityUser
{
    public required string DisplayName { get; set; }

}
