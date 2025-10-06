namespace App.Config;

public class JwtConfig
{
    public required string ValidAudience { get; set; } = "";
    public required string ValidIssuer { get; set; } = "";
    public required string Secret { get; set; } = "";
    public float ExpirationMins { get; set; } = 3 * 60;
}
