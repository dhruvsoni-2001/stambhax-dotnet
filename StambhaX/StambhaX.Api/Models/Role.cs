namespace StambhaX.Api.Models;

public class Role : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }

    // Relationships
    public List<UserRole> UserRoles { get; set; } = new();
}
