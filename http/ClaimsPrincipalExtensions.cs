namespace TaskBoard.Api.Http;

using System.Security.Claims;

public static class ClamsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        string value = user.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new InvalidOperationException("Brak identyfikatora uzytkownika.");
        return Guid.Parse(value);
    }
}