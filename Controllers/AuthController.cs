using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Api.Contracts;
using TaskBoard.Api.Domain;
using TaskBoard.Api.Http;
using TaskBoard.Api.Services;

namespace TaskBoard.Api.Controllers;


[Controller]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _service;

    public AuthController(AuthService service)
    {
        this._service = service;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<CurrentUserResponse>> Register(RegisterUserRequest request)
    {
        var user = await _service.RegisterAsync(request.email.Value, request.password);
        await SignInUserAsync(user);
        return CreatedAtAction(nameof(Me), new { user.Id }, CurrentUserResponse.From(user));
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<CurrentUserResponse>> Login(LoginUserRequest request)
    {
        var user = await _service.ValidateCredentialsAsync(request.email.Value, request.password);
        if (user is null)
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized, title: "Nieprawidlowy email lub haslo");
        }
        await SignInUserAsync(user);
        return Ok(CurrentUserResponse.From(user));
    }

    [AllowAnonymous]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return NoContent();
    }

    [HttpGet("me")]
    public async Task<ActionResult<CurrentUserResponse>> Me(ClaimsPrincipal userClaims)
    {
        var user = await _service.GetByIdAsync(userClaims.GetUserId());
        if (user is null)
        {
            return Unauthorized();
        }
        return CurrentUserResponse.From(user);
    }

    private async Task SignInUserAsync(User user)
    {
        Claim[] claims = [ new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), new Claim(ClaimTypes.Email, user.Email.Value) ];
        ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
    } 
}