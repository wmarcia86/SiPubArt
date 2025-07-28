using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics;

namespace Web.API.Common;

/// <summary>
/// Factory class for creating ProblemDetails and ValidationProblemDetails for the Web API.
/// Type: Factory
/// Author: YourName
/// Date: 2025-07-28
/// </summary>
public class WebAPIProblemDetailsFactory : ProblemDetailsFactory
{
    private readonly ApiBehaviorOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="WebAPIProblemDetailsFactory"/> class.
    /// </summary>
    /// <param name="options">The API behavior options to use for error mapping.</param>
    public WebAPIProblemDetailsFactory(ApiBehaviorOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// Creates a <see cref="ProblemDetails"/> instance for general errors.
    /// </summary>
    /// <param name="httpContext">The current HTTP context.</param>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <param name="title">The title for the problem details.</param>
    /// <param name="type">The type URI for the problem details.</param>
    /// <param name="detail">The detail message.</param>
    /// <param name="instance">The instance URI.</param>
    /// <returns>A <see cref="ProblemDetails"/> object.</returns>
    public override ProblemDetails CreateProblemDetails(
        HttpContext httpContext,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null)
    {
        statusCode ??= 500;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Type = type,
            Detail = detail,
            Instance = instance
        };

        ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);

        return problemDetails;
    }

    /// <summary>
    /// Creates a <see cref="ValidationProblemDetails"/> instance for validation errors.
    /// </summary>
    /// <param name="httpContext">The current HTTP context.</param>
    /// <param name="modelStateDictionary">The model state dictionary containing validation errors.</param>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <param name="title">The title for the problem details.</param>
    /// <param name="type">The type URI for the problem details.</param>
    /// <param name="detail">The detail message.</param>
    /// <param name="instance">The instance URI.</param>
    /// <returns>A <see cref="ValidationProblemDetails"/> object.</returns>
    public override ValidationProblemDetails CreateValidationProblemDetails(
        HttpContext httpContext,
        ModelStateDictionary modelStateDictionary,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null)
    {
        if (modelStateDictionary == null)
        {
            throw new ArgumentNullException(nameof(modelStateDictionary));
        }

        statusCode ??= 400;

        var problemDetails = new ValidationProblemDetails(modelStateDictionary)
        {
            Status = statusCode,
            Type = type,
            Detail = detail,
            Instance = instance
        };

        if (title != null)
        {
            problemDetails.Title = title;
        }

        ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);

        return problemDetails;

    }

    /// <summary>
    /// Applies default values and additional information to the given <see cref="ProblemDetails"/> instance.
    /// </summary>
    /// <param name="httpContext">The current HTTP context.</param>
    /// <param name="problemDetails">The problem details object to update.</param>
    /// <param name="statusCode">The HTTP status code.</param>
    private void ApplyProblemDetailsDefaults(HttpContext httpContext, ProblemDetails problemDetails, int statusCode)
    {
        problemDetails.Status ??= statusCode;

        if (_options.ClientErrorMapping.TryGetValue(statusCode, out var clientErrorData))
        {
            problemDetails.Title ??= clientErrorData.Title;
            problemDetails.Type ??= clientErrorData.Link;
        }

        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        if (traceId != null)
        {
            problemDetails.Extensions["traceId"] = traceId;
        }

        var errors = httpContext?.Items[HttpContextItemKeys.Erros] as List<Error>;

        if (errors is not null)
        {
            problemDetails.Extensions.Add("errorCodes", errors.Select(e => e.Code));
        }
    }
}