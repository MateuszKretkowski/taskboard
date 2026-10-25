using TaskBoard.Api.Domain;

namespace TaskBoard.Api.Contracts;

public record CurrentUserResponse(Guid Id, string email)
{
    public static CurrentUserResponse From(User user)
    {
        return new CurrentUserResponse(user.Id, user.Email.Value);
    }
}