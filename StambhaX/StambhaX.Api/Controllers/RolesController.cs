using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StambhaX.Api.Data;
using StambhaX.Api.DTOs;
using StambhaX.Api.Models;

namespace StambhaX.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class RolesController : ControllerBase
{
    private readonly AppDbContext _context;

    public RolesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _context.Roles
            .Where(r => !r.IsDeleted)
            .Select(r => new RoleDto(r.Id, r.Name, r.Description))
            .ToListAsync();

        return Ok(roles);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto request)
    {
        if (await _context.Roles.AnyAsync(r => r.Name == request.Name))
            return BadRequest("Role already exists.");

        var role = new Role { Name = request.Name, Description = request.Description };
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRoles), new { id = role.Id }, new RoleDto(role.Id, role.Name, role.Description));
    }
}
