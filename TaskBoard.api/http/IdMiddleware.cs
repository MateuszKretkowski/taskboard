namespace TaskBoard.Api.Http;

public class IdMiddleware
{
    private readonly RequestDelegate _next;
    public string header = "X-Request-Id";

    public IdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (Microsoft.Extensions.Primitives.StringValues.IsNullOrEmpty(context.Request.Headers[header]))
        {
            Guid id = Guid.CreateVersion7();
            context.Response.Headers[header] = id.ToString();
        }
        else
        {
            context.Response.Headers[header] = context.Request.Headers[header];
        }

        await _next(context);
    }
}