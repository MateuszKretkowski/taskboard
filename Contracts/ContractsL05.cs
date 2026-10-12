using System.ComponentModel.DataAnnotations;
using TaskBoard.Api.Domain;

public record ProjectCreateRequest(
    [Required, MaxLength(100)] string Title
);

public record ProjectRenameRequest(
    [Required, MaxLength(100)] string Title
);

public record ProjectResponse(
    Guid Id, 
    string Title,
    bool Archived,
    DateTime CreatedAt,
    Guid OwnerId,
    int Counter
);