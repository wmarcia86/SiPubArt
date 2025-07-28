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

[ApiController]
[Route("api/users")]
public class UsersController : ApiController
{
    private readonly ISender _mediator;

    public UsersController(ISender mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

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
