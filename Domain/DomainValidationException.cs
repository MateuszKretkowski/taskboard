namespace TaskBoard.Api.Domain;

public class DomainValidationException : DomainException
{
    public DomainValidationException(string message) : base(message)
    {
        
    }
}