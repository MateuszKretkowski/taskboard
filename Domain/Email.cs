namespace TaskBoard.Api.Domain;

public record Email
{
    public static int MaxValueLength = 254;

    public string Value {get;}

    public Email(string value)
    {
        string? error = Validate(value, out string normalized);
        if (error is not null)
        {
            throw new DomainValidationException(error);
        }

        Value = normalized;
    }

    public static bool TryParse(string? value, out Email? email)
    {
        if (Validate(value, out _) is not null)
        {
            email = null;
            return false;
        }

        email = new Email(value!);
        return true;
    }

    private static string? Validate(string? value, out string normalized)
    {
        normalized = "";
        if (string.IsNullOrWhiteSpace(value))
        {
            return "Email nie moze byc pusty.";
        }
        normalized = value.Trim().ToLowerInvariant();
        if (normalized.Length > MaxValueLength)
        {
            return $"Email nie moze byc dluzszy niz: {MaxValueLength}.";
        }
        int indexOfAt = normalized.IndexOf("@");
        bool isProper = indexOfAt >= 0 && indexOfAt == normalized.LastIndexOf("@");
        if (!isProper || indexOfAt == 0 || indexOfAt == normalized.Length - 1)
        {
            return "Email musi miec @";
        }
        return null;
    }
}