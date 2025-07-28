using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Web.API.Common;

namespace Web.API.Controllers.Base;

/// <summary>
/// Base controller class providing common functionality for API controllers.
/// Type: Base Controller
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
[ApiController]
public class ApiController : ControllerBase
{
    /// <summary>
    /// Returns a problem response based on a list of errors.
    /// </summary>
    /// <param name="errors">The list of errors to process.</param>
    /// <returns>An appropriate IActionResult based on the error types.</returns>
    protected IActionResult Problem(List<Error> errors)
    {
        if (errors.Count is 0)
        {
            return Problem();
        }

        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }

        HttpContext.Items[HttpContextItemKeys.Erros] = errors;

        return Problem(errors[0]);
    }

    /// <summary>
    /// Returns a problem response for a single error.
    /// </summary>
    /// <param name="error">The error to process.</param>
    /// <returns>An IActionResult with the appropriate status code and error description.</returns>
    private IActionResult Problem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError,
        };

        return Problem(statusCode: statusCode, title: error.Description);
    }

    /// <summary>
    /// Returns a validation problem response based on a list of validation errors.
    /// </summary>
    /// <param name="errors">The list of validation errors.</param>
    /// <returns>An IActionResult representing the validation problem.</returns>
    private IActionResult ValidationProblem(List<Error> errors)
    {
        var modelStateDictionary = new ModelStateDictionary();

        foreach (var error in errors)
        {
            modelStateDictionary.AddModelError(error.Code, error.Description);
        }

        return ValidationProblem(modelStateDictionary);
    }

    /// <summary>
    /// Gets the user's role from the current claims principal.
    /// </summary>
    /// <returns>The user's role as a string, or an empty string if not found.</returns>
    protected string UserRole()
    {
        var role = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value;

        return role ?? string.Empty;
    }
}