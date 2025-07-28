using Application.Users.Create;
using Application.Users.Delete;
using Application.Users.GetAll;
using Application.Users.GetById;
using Application.Users.Update;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Web.API.Controllers.Base;

namespace Web.API.Controllers;

/// <summary>
/// Controller class for managing user-related API endpoints.
/// Type: Controller
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
[ApiController]
[Route("api/users")]
public class UsersController : ApiController
{
    private readonly ISender _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsersController"/> class.
    /// </summary>
    /// <param name="mediator">The mediator instance for handling requests.</param>
    public UsersController(ISender mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="command">The command containing user creation data.</param>
    /// <returns>The result of the creation operation.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
    {
        var createResult = await _mediator.Send(command);

        return createResult.Match(
            userId => Ok(userId),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>The requested user or an error.</returns>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var getByIdResult = await _mediator.Send(new GetUserByIdQuery(id));

        return getByIdResult.Match(
            user => Ok(user),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Retrieves all users.
    /// </summary>
    /// <returns>A list of all users or an error.</returns>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [EnableQuery]
    public async Task<IActionResult> GetAll()
    {
        var getAllResult = await _mediator.Send(new GetAllUsersQuery());

        return getAllResult.Match(
            users => Ok(users),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="id">The unique identifier of the user to update.</param>
    /// <param name="command">The command containing updated user data.</param>
    /// <returns>The result of the update operation.</returns>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserCommand command)
    {
        if (command.Id != id)
        {
            List<Error> errors = new()
            {
                Error.Validation("User.UpdateInvalid", "The request Id does not match with the url Id.")
            };

            return Problem(errors);
        }

        var updateResult = await _mediator.Send(command);

        return updateResult.Match(
            userId => Ok(userId),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Deletes a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user to delete.</param>
    /// <returns>No content if successful, or an error.</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleteResult = await _mediator.Send(new DeleteUserCommand(id));

        return deleteResult.Match(
            userId => NoContent(),
            errors => Problem(errors)
        );
    }
}
