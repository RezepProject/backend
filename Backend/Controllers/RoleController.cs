using backend.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Controllers.Validators;
using FluentValidation;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class RoleController : GenericController<Role, int>
    {
        public RoleController(DataContext ctx) : base(ctx)
        {
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

            return CreatedAtAction("GetEntity", new { id = newRole.Entity.Id }, role);
        }
    }
}