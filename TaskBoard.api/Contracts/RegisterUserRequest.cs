using System.ComponentModel.DataAnnotations;
using TaskBoard.Api.Domain;

namespace TaskBoard.Api.Contracts;

public record RegisterUserRequest([Required] Email email, [Required] string password);