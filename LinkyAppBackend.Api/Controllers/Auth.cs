using LinkyAppBackend.Api.Models.Dtos.Auth;
using LinkyAppBackend.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LinkyAppBackend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Auth(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<TokenDto>> Register([FromBody] RegisterDto dto)
    {
        return Ok(await authService.SignUp(dto));
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenDto>> Login([FromBody] LoginDto dto)
    {
        return Ok(await authService.SignIn(dto));
    }
    
    [HttpPost("google-login")]
    public async Task<ActionResult<TokenDto>> GoogleLogin([FromBody] ExternalAuthDto dto)
    {
        return Ok(await authService.GoogleSignIn(dto));
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<TokenDto>> Refresh([FromBody] string refreshToken)
    {
        return Ok(await authService.Refresh(refreshToken));
    }
}