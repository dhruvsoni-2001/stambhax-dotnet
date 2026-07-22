using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StambhaX.Api.Data;
using StambhaX.Api.DTOs;
using StambhaX.Api.Models;
using AutoMapper;
using StambhaX.Api.Repositories;

namespace StambhaX.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class RolesController : ControllerBase
{
    //private readonly AppDbContext _context;
    private readonly IRepository<Role> _roleRepository;
    private readonly IMapper _mapper;

    public RolesController(IRepository<Role> roleRepsitory, IMapper mapper)
    {
        _roleRepository = roleRepsitory;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _roleRepository.FindAsync(r => !r.IsDeleted);

        var roleDtos = _mapper.Map<List<RoleDto>>(roles);

        return Ok(roleDtos);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto request)
    {
        if (await _roleRepository.FindAsync(r => r.Name == request.Name && !r.IsDeleted) is { } existingRoles && existingRoles.Any())
            return BadRequest("Role name already exists.");
        var role = new Role
        {
            Name = request.Name,
            Description = request.Description
        };
        await _roleRepository.AddAsync(role);
        await _roleRepository.SaveChangesAsync();
        var roleDto = _mapper.Map<RoleDto>(role);
        return CreatedAtAction(nameof(GetRoles), new { id = role.Id }, roleDto);
    }
}
