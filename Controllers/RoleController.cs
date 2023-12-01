using backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class RoleController : ControllerBase
{
    private readonly DataContext _ctx;

    public RoleController(DataContext ctx)
    {
        _ctx = ctx;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
    {
        return await _ctx.Roles.ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Role>> GetRole(int id)
    {
        var role = await _ctx.Roles.FindAsync(id);
        return role == null ? NotFound("Role id not found!") : role;
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateRole(int id, CreateRole newRole)
    {
        var role = await _ctx.Roles.FindAsync(id);
        if (role == null)
        {
            return NotFound("Role id not found!");
        }

        role.Name = newRole.Name;
        _ctx.Roles.Update(role);

        try
        {
            await _ctx.SaveChangesAsync();
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
        var newRole = _ctx.Roles.Add(new Role { Name = role.Name });
        await _ctx.SaveChangesAsync();

        return CreatedAtAction("GetRole", new { id = newRole.Entity.Id }, role);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRole(int id)
    {
        var role = await _ctx.Roles.FindAsync(id);
        if (role == null)
        {
            return NotFound("Role id not found!");
        }

        _ctx.Roles.Remove(role);
        await _ctx.SaveChangesAsync();
        return NoContent();
    }
}