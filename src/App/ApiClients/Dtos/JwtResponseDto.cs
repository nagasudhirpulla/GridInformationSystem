namespace App.ApiClients.Dtos;

public class JwtResponseDto
{
    public required string Token { get; set; }
    public required DateTime Expiration { get; set; }
}
