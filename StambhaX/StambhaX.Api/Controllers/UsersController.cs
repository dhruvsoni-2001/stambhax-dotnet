using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Isopoh.Cryptography.Argon2;
using StambhaX.Api.Data;
using StambhaX.Api.DTOs;
using StambhaX.Api.Models;

namespace StambhaX.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Requires authentication
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Where(u => !u.IsDeleted)
            .Select(u => new UserDto(
                u.Id, u.Username, u.Email, u.IsActive, u.TwoFactorEnabled,
                u.UserRoles.Select(ur => ur.Role!.Name).ToList(), u.CreatedAt))
            .ToListAsync();

        return Ok(users);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto request)
    {
        if (await _context.Users.AnyAsync(u => u.Email == request.Email || u.Username == request.Username))
            return BadRequest("Email or Username already exists.");

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = Argon2.Hash(request.Password),
            IsActive = true
        };

        var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
        if (userRole != null)
        {
            user.UserRoles.Add(new UserRole { RoleId = userRole.Id });
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var roles = user.UserRoles.Select(ur => userRole?.Name ?? "").Where(n => !string.IsNullOrEmpty(n)).ToList();
        var userDto = new UserDto(user.Id, user.Username, user.Email, user.IsActive, user.TwoFactorEnabled, roles, user.CreatedAt);

        return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, userDto);
    }
}
