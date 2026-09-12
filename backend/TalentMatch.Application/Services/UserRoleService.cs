using TalentMatch.Application.Dtos;
using TalentMatch.Application.Interfaces;

namespace TalentMatch.Application.Services;

public class UserRoleService : IUserRoleService
{
    public Task<RoleClaimsDto> GetRoleByUserNameAsync(string userName)
    {
        var normalizedUser = userName.Trim();

        var role = normalizedUser.Equals("admin", StringComparison.OrdinalIgnoreCase)
            ? "Admin"
            : "User";

        return Task.FromResult(new RoleClaimsDto
        {
            UserName = normalizedUser,
            Role = role
        });
    }
}
