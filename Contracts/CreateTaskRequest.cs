using System.ComponentModel.DataAnnotations;
using TaskBoard.Api.Domain;

namespace TaskBoard.Api.Contracts;

public record CreateTaskRequest([Required, MaxLength(200)] string Title);