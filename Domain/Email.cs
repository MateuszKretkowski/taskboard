namespace TaskBoard.Api.Domain;

public record Email
{
    public static int MaxValueLength = 254;

    public string Value {get;}

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Email nie moze byc pusty.", nameof(value));
        }
        var normalized = value.Trim().ToLowerInvariant();
        if (normalized.Length > MaxValueLength)
        {
            throw new ArgumentException($"Email nie moze byc dluzszy niz: {MaxValueLength}", nameof(value));
        }
        int indexOfAt = normalized.IndexOf("@");
        bool isProper = indexOfAt >= 0 && indexOfAt == normalized.LastIndexOf("@");
        if (!isProper || indexOfAt == 0 || indexOfAt == normalized.Length - 1)
        {
            throw new ArgumentException("Email musi miec @", nameof(value));
        }

        Value = normalized;
    }
}