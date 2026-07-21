namespace StambhaX.Api.DTOs;

public record UserDto(
    Guid Id,
    string Username,
    string Email,
    bool IsActive,
    bool TwoFactorEnabled,
    List<string> Roles,
    DateTime CreatedAt
);

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
