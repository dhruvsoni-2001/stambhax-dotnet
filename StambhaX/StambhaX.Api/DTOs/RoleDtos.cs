namespace StambhaX.Api.DTOs;

public class RoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public record CreateRoleDto(
    string Name,
    string? Description
);
