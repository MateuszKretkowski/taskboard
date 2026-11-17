using System.ComponentModel.DataAnnotations;
using TaskBoard.Api.Domain;

namespace TaskBoard.Api.Contracts;

public record RegisterUserRequest([Required] string email, [Required] string password);