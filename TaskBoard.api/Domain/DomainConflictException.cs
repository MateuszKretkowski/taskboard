namespace TaskBoard.Api.Domain;

public class DomainConflictException : DomainException
{
    public DomainConflictException(string message) : base(message)
    {
        
    }
}