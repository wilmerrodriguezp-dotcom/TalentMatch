namespace TalentMatch.Application.Dtos;

public class TokenResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
}
