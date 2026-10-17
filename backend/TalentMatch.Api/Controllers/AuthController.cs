using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentMatch.Application.Dtos;
using TalentMatch.Application.Interfaces;

namespace TalentMatch.Api.Controllers;

[ApiController]
[Route("api/v1")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var token = await _authService.LoginAsync(request, cancellationToken);
        return Ok(token);
    }

    [HttpGet("admin-check")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult AdminCheck()
    {
        return Ok(new { mensaje = "Acceso permitido para administradores." });
    }

    [HttpGet("user-check")]
    [Authorize(Policy = "UserOrAdmin")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult UserCheck()
    {
        return Ok(new { mensaje = "Acceso permitido para usuarios y administradores." });
    }
}
