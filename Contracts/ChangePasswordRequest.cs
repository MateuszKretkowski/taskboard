namespace TaskBoard.Api.Contracts;

public record ChangePasswordRequest(string currentPassword, string newPassword);