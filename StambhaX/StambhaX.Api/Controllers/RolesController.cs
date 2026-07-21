using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StambhaX.Api.Data;
using StambhaX.Api.DTOs;
using StambhaX.Api.Models;
using AutoMapper;

namespace StambhaX.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class RolesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public RolesController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _context.Roles
            .Where(r => !r.IsDeleted)
            .ToListAsync();

        var roleDtos = _mapper.Map<List<RoleDto>>(roles);

        return Ok(roleDtos);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto request)
    {
        if (await _context.Roles.AnyAsync(r => r.Name == request.Name))
            return BadRequest("Role already exists.");

        var role = new Role { Name = request.Name, Description = request.Description };
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        var roleDto = _mapper.Map<RoleDto>(role);

        return CreatedAtAction(nameof(GetRoles), new { id = role.Id }, roleDto);
    }
}
