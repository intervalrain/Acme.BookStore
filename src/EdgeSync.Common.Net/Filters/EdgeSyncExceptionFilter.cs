using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using EdgeSync.Common.Net.Exceptions;
using Microsoft.AspNetCore.Http;

namespace EdgeSync.Common.Net.Filters;

/// <summary>
/// Exception filter for EdgeSync exceptions - converts to RFC 9457 Problem Details
/// </summary>
public class EdgeSyncExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var (statusCode, type, title) = context.Exception switch
        {
            EdgeSyncAuthorizationException => (StatusCodes.Status401Unauthorized, "https://example.com/probs/unauthorized", "Unauthorized"),
            EdgeSyncBadRequestException => (StatusCodes.Status400BadRequest, "https://example.com/probs/bad-request", "Bad Request"),
            EdgeSyncForbiddenException => (StatusCodes.Status403Forbidden, "https://example.com/probs/forbidden", "Forbidden"),
            EdgeSyncNotFoundException => (StatusCodes.Status404NotFound, "https://example.com/probs/not-found", "Not Found"),
            EdgeSyncConflictException => (StatusCodes.Status409Conflict, "https://example.com/probs/conflict", "Conflict"),
            EdgeSyncUnprocessableEntityException => (StatusCodes.Status422UnprocessableEntity, "https://example.com/probs/validation-error", "Unprocessable Entity"),
            _ => (StatusCodes.Status500InternalServerError, "https://example.com/probs/internal-server-error", "Internal Server Error")
        };

        
        var problemDetails = new ProblemDetails
        {
            Type = type,
            Title = title,
            Status = statusCode,
            Detail = context.Exception.Message,
            Instance = context.HttpContext.Request.Path,
        };

        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = statusCode,
            ContentTypes = { "application/problem+json" }
        };

        context.ExceptionHandled = true;
    }
}
