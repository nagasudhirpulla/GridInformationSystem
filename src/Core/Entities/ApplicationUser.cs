using Microsoft.AspNetCore.Identity;

namespace Core.Entities;

public class ApplicationUser : IdentityUser
{
    public required string DisplayName { get; set; }

}
