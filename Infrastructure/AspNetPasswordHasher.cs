using Microsoft.AspNetCore.Identity;
using TaskBoard.Api.Services;

namespace TaskBoard.Api.Infrastructure;

public class AspNetPasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<object> _inner = new ();

    public string Hash(string password) => _inner.HashPassword(null!, password);

    public bool Verify(string passwordHash, string password) => _inner.VerifyHashedPassword(null!, passwordHash, password) != PasswordVerificationResult.Failed;
}