using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Controllers.Base;

/// <summary>
/// Controller class for handling global error responses.
/// Type: Controller
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
[ApiController]
[Route("[controller]")]
public class ErrorsControler : ControllerBase
{
    /// <summary>
    /// Handles unhandled exceptions and returns a problem response.
    /// </summary>
    /// <returns>An IActionResult representing the error response.</returns>
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/error")]
    public IActionResult Error()
    {
        Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        return Problem();
    }
}