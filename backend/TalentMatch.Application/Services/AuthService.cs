using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TalentMatch.Application.Dtos;
using TalentMatch.Application.Interfaces;

namespace TalentMatch.Application.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRoleService _userRoleService;

    public AuthService(IConfiguration configuration, IUserRoleService userRoleService)
    {
        _configuration = configuration;
        _userRoleService = userRoleService;
    }

    public async Task<TokenResponseDto> LoginAsync(LoginRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("Usuario y contraseña son obligatorios.");

        var validCredentials =
            (request.UserName.Equals("admin", StringComparison.OrdinalIgnoreCase) && request.Password == "123456") ||
            (request.UserName.Equals("user", StringComparison.OrdinalIgnoreCase) && request.Password == "123456");

        if (!validCredentials)
            throw new UnauthorizedAccessException("Credenciales inválidas.");

        var roleInfo = await _userRoleService.GetRoleByUserNameAsync(request.UserName);
        var key = _configuration["Jwt:Key"] ?? "SuperSecretKeyForTalentMatchProject1234567890";
        var issuer = _configuration["Jwt:Issuer"] ?? "TalentMatch";
        var audience = _configuration["Jwt:Audience"] ?? "TalentMatchUsers";

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, request.UserName),
            new Claim(ClaimTypes.Name, request.UserName),
            new Claim(ClaimTypes.Role, roleInfo.Role)
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddHours(2);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new TokenResponseDto
        {
            Token = tokenString,
            Expiration = expiration
        };
    }
}
