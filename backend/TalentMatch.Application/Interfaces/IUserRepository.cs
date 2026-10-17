using TalentMatch.Application.Dtos;

namespace TalentMatch.Application.Interfaces;

public interface IUserRepository
{
    Task<UserCredentialsDto?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
    Task CreateAsync(UserCredentialsDto user, CancellationToken cancellationToken = default);
}
