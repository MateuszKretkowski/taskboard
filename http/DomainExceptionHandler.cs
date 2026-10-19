using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Api.Domain;

namespace TaskBoard.Api.Http;

public class DomainExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetails;
    private readonly ILogger<DomainExceptionHandler> _logger;

    public DomainExceptionHandler(IProblemDetailsService problemDetails, ILogger<DomainExceptionHandler> logger)
    {
        this._problemDetails = problemDetails;
        this._logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToekn)
    {
        if (exception is not DomainException domainException)
        {
            return false;
        }

        int status = domainException is DomainConflictException ? StatusCodes.Status409Conflict : StatusCodes.Status400BadRequest;

        _logger.LogInformation("Odrzucono zadanie {method} {path}: {reason} ");

        httpContext.Response.StatusCode = status;
        return await _problemDetails.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Status = status,
                Title = status == StatusCodes.Status409Conflict ? "Operacja niedozwolona w obecnym stanie." : "Niepoprawne dane",
                Detail = domainException.Message
            }
        });
    }
}