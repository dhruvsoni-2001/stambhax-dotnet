using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Isopoh.Cryptography.Argon2;
using StambhaX.Api.Data;
using StambhaX.Api.DTOs;
using StambhaX.Api.Services;
using AutoMapper;

namespace StambhaX.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly JwtService _jwtService;
    private readonly IMapper _mapper;

    public AuthController(AppDbContext context, JwtService jwtService, IMapper mapper)
    {
        _context = context;
        _jwtService = jwtService;
        _mapper = mapper;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == request.Email && !u.IsDeleted);

        if (user == null || !Argon2.Verify(user.PasswordHash, request.Password))
            return Unauthorized("Invalid credentials.");

        if (!user.IsActive)
            return Forbid("User account is disabled.");

        var roles = user.UserRoles.Select(ur => ur.Role!.Name).ToList();
        var token = _jwtService.GenerateToken(user, roles);

        // Set HttpOnly Cookie
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true, // Set to false if running locally on HTTP
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddMinutes(60) // Match JWT Expiry
        };
        Response.Cookies.Append("X-Access-Token", token, cookieOptions);

        var userDto = _mapper.Map<UserDto>(user);

        return Ok(new AuthResponseDto(token, userDto));
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("X-Access-Token");
        return Ok(new { message = "Logged out successfully" });
    }
}
