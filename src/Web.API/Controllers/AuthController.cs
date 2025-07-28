using Application.Users.Create;
using Application.Users.Login;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.API.Controllers.Base;

namespace Web.API.Controllers;

/// <summary>
/// Controller class for handling authentication-related API endpoints.
/// Type: Controller
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController : ApiController
{
    private readonly ISender _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="mediator">The mediator instance for handling requests.</param>
    public AuthController(ISender mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="command">The command containing user registration data.</param>
    /// <returns>The result of the registration operation.</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    { 
        var registerResult = await _mediator.Send(command);

        return registerResult.Match(
            userId => Ok(userId),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Authenticates a user and returns user information.
    /// </summary>
    /// <param name="command">The command containing login credentials.</param>
    /// <returns>The authenticated user or an error.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        var loginResult = await _mediator.Send(command);

        return loginResult.Match(
            user => Ok(user),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Retrieves information about the currently authenticated user.
    /// </summary>
    /// <returns>The current user's information.</returns>
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