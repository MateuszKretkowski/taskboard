namespace TaskBoard.Api.Http;

public class MaintenanceMiddleware
{
    public RequestDelegate _next;
    bool isMaintenance;

    public MaintenanceMiddleware(RequestDelegate next)
    {
        _next = next;
        isMaintenance = Environment.GetEnvironmentVariable("TASKBOARD_MAINTENANCE") == "1";
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            if (isMaintenance && !context.Request.Path.StartsWithSegments("/health"))
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                context.Response.Headers.RetryAfter = "120";
                await context.Response.WriteAsync("Trwa przerwa techniczna");
            }
            
            await _next(context);
        }
        finally
        {

        }
    }
}