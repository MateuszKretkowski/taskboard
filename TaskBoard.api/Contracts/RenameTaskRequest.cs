using System.ComponentModel.DataAnnotations;
using TaskBoard.Api.Domain;

namespace TaskBoard.Api.Contracts;

public record RenameTaskRequest([Required, MaxLength(200)] string Title);