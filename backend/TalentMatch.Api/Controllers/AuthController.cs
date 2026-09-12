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
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var token = await _authService.LoginAsync(request);
        return Ok(token);
    }
}
