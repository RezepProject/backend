using backend.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Controllers.Validators;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class RoleController : ControllerBase
{
    private readonly DataContext ctx;

    public RoleController(DataContext ctx)
    {
        this.ctx = ctx;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
    {
        return await ctx.Roles.ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Role>> GetRole(int id)
    {
        var role = await ctx.Roles.FindAsync(id);
        return role == null ? NotFound("Role id not found!") : role;
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateRole(int id, CreateRole newRole)
    {
        var validator = new CreateRoleValidator();
        var validationResult = await validator.ValidateAsync(newRole);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var role = await ctx.Roles.FindAsync(id);
        if (role == null) return NotFound("Role id not found!");

        role.Name = newRole.Name;
        ctx.Roles.Update(role);

        try
        {
            await ctx.SaveChangesAsync();
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
        var validator = new CreateRoleValidator();
        var validationResult = await validator.ValidateAsync(role);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var newRole = ctx.Roles.Add(new Role { Name = role.Name });
        await ctx.SaveChangesAsync();

        return CreatedAtAction("GetRole", new { id = newRole.Entity.Id }, role);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRole(int id)
    {
        var role = await ctx.Roles.FindAsync(id);
        if (role == null) return NotFound("Role id not found!");

        ctx.Roles.Remove(role);
        await ctx.SaveChangesAsync();
        return NoContent();
    }
}
