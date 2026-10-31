using TaskBoard.Api.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TaskBoard.Api.Http;

public class ConcurrencyExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetails;
    private readonly ILogger<ConcurrencyExceptionHandler> _logger;

    public ConcurrencyExceptionHandler(IProblemDetailsService problemDetails, ILogger<ConcurrencyExceptionHandler> logger)
    {
        this._problemDetails = problemDetails;
        this._logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ConcurrencyConflictException conflict)
        {
            return false;
        }

        _logger.LogWarning("Konflikt wspolbieznosci przy {method} {path}", httpContext.Request.Method, httpContext.Request.Path);

        httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
        return await _problemDetails.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Konflikt wspolbieznej zmiany.",
                Detail = conflict.Message
            }
        });
    }
}