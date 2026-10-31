using System.Diagnostics;

namespace TaskBoard.Api.Http;

public class RequestTimingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestTimingMiddleware(RequestDelegate _next)
    {
        this._next = _next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        
        context.Response.OnStarting(() =>
        {
            context.Response.Headers["X-Elapsed-Ms"] = stopwatch.ElapsedMilliseconds.ToString();
            return Task.CompletedTask;
        });

        try
        {
            await _next(context);
        }
        finally
        {
            string outcome = context.RequestAborted.IsCancellationRequested
            ? "przerwane przez klienta" : context.Response.StatusCode.ToString();
            Console.WriteLine($"{context.Request.Method}, {context.Request.Path} -> {outcome}, ({stopwatch.ElapsedMilliseconds}ms)");
        }
    }
}