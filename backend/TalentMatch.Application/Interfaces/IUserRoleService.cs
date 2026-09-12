using TalentMatch.Application.Dtos;

namespace TalentMatch.Application.Interfaces;

public interface IUserRoleService
{
    Task<RoleClaimsDto> GetRoleByUserNameAsync(string userName);
}
