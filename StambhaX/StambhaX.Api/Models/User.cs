namespace StambhaX.Api.Models;

public class User : BaseEntity
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public bool IsActive { get; set; } = true;

    // Security & Auth extensions (for future TOTP / 2FA / Recovery)
    public bool TwoFactorEnabled { get; set; } = false;
    public string? TotpSecret { get; set; }
    public List<string> RecoveryCodes { get; set; } = new();

    // Relationships
    public List<UserRole> UserRoles { get; set; } = new();
}
