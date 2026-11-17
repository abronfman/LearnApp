using LearnApp.Api.Auth;
using LearnApp.Api.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnApp.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IDemoUserStore users, ITokenService tokens) : ControllerBase
{

    // POST api/auth/login -> 200 with token, or 401
    [AllowAnonymous]
    [HttpPost("login")]
    public ActionResult<LoginResponse> Login(LoginRequest request)
    {
        var user = users.FindByCredentials(request.Email, request.Password);

        if (user is null)
            return Unauthorized();

        var (token, expires) = tokens.CreateToken(user);
        return Ok(new LoginResponse(token, expires, [user.Role]));
    }

    // GET api/auth/me -> 200 with user info, or 401
    [HttpGet("me")]
    public ActionResult<MeResponse> Me() => Ok(new MeResponse(
        User.Identity?.Name,
        User.FindFirst("email")?.Value,
        User.FindAll("role").Select(c => c.Value).ToList()));
}
