using backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class RoleController : ControllerBase
{
    private readonly DataContext _context;

    public RoleController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
    {
        return await _context.Roles.ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Role>> GetRole(int id)
    {
        var role = await _context.Roles.FindAsync(id);
        return role == null ? NotFound("Role id not found!") : role;
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateRole(int id, CreateRole newRole)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null)
        {
            return NotFound("Role id not found!");
        }

        role.Name = newRole.Name;
        _context.Roles.Update(role);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Role>> CreateRole(CreateRole role)
    {
        var newRole = _context.Roles.Add(new Role { Name = role.Name });
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetRole", new { id = newRole.Entity.Id }, role);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRole(int id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null)
        {
            return NotFound("Role id not found!");
        }

        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}