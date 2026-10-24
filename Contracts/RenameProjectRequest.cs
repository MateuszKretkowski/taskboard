using System.ComponentModel.DataAnnotations;
using TaskBoard.Api.Domain;

namespace TaskBoard.Api.Contracts;

public record RenameProjectReqeust([Required, MaxLength(Project.MaxNameLength)] string name);