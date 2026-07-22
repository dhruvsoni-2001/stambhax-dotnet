namespace StambhaX.Application.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public List<string> Roles { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

public record CreateUserDto(
    string Username,
    string Email,
    string Password
);

public record UpdateUserDto(
    string? Username,
    string? Email,
    bool? IsActive
);
