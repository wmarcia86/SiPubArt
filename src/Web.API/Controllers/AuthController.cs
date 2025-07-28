using Application.Users.Create;
using Application.Users.Login;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.API.Controllers.Base;

namespace Web.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ApiController
{
    private readonly ISender _mediator;

    public AuthController(ISender mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    { 
        var registerResult = await _mediator.Send(command);

        return registerResult.Match(
            userId => Ok(userId),
            errors => Problem(errors)
        );
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        var loginResult = await _mediator.Send(command);

        return loginResult.Match(
            user => Ok(user),
            errors => Problem(errors)
        );
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        var id = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var fullName = User.Claims.FirstOrDefault(c => c.Type == "fullName")?.Value;
        var username = User.Identity?.Name;
        var email = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
        var role = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value;

        return Ok(new
        {
            Id = id,
            FullName = fullName,
            Username = username,
            Email = email,
            Role = role
        });
    }
}