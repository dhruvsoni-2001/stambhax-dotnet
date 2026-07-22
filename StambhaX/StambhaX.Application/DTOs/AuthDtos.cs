namespace StambhaX.Application.DTOs;

public record LoginRequestDto(
    string Email,
    string Password
);

public record AuthResponseDto(
    string Token,
    UserDto User
);
