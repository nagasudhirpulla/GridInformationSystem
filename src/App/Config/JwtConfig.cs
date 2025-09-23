using System.ComponentModel.DataAnnotations;

namespace App.Config;

public class JwtConfig
{
    [Required]
    public string ValidAudience { get; set; } = "";
    [Required]
    public string ValidIssuer { get; set; } = "";
    [Required]
    public string Secret { get; set; } = "";
    public float ExpirationMins { get; set; } = 3 * 60;
}
