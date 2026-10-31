using System.ComponentModel.DataAnnotations;
using TaskBoard.Api.Domain;

namespace TaskBoard.Api.Contracts;

public record LoginUserRequest([Required] Email email, [Required] string password);