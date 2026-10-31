namespace TaskBoard.Api.Services;

public class ConcurrencyConflictException: Exception
{
    public ConcurrencyConflictException(string message, Exception innerException) : base(message,innerException)
    {
        
    }
}