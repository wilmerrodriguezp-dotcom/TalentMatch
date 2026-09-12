using TalentMatch.Application.Dtos;

namespace TalentMatch.Application.Interfaces;

public interface IAuthService
{
    Task<TokenResponseDto> LoginAsync(LoginRequestDto request);
}
