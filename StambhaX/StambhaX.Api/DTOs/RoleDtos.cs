namespace StambhaX.Api.DTOs;

public record RoleDto(
    Guid Id,
    string Name,
    string? Description
);

public record CreateRoleDto(
    string Name,
    string? Description
);
