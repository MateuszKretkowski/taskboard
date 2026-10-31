using System.ComponentModel.DataAnnotations;
using TaskBoard.Api.Domain;

namespace TaskBoard.Api.Contracts;

public record TaskListQuery(TaskItemStatus? status, [Range(1, int.MaxValue)] int page = 1, [Range(1, 100)] int pageSize = 20);