using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Web.API.Middlewares;

/// <summary>
/// Middleware for handling global exceptions and returning standardized error responses.
/// Type: Middleware
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
public class GloblalExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<GloblalExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GloblalExceptionHandlingMiddleware"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging errors.</param>
    public GloblalExceptionHandlingMiddleware(ILogger<GloblalExceptionHandlingMiddleware> logger) => _logger = logger;

    /// <summary>
    /// Invokes the middleware to handle exceptions and return a problem details response.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);

            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

            ProblemDetails problemDetails = new()
            {
                Status = (int) HttpStatusCode.InternalServerError,
                Type = "Server Error",
                Title = "Server Error",
                Detail = "An internal server has ocurred."
            };

            string json = JsonSerializer.Serialize(problemDetails);

            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(json);
        }
    }
}
