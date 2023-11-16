using backend.Entities;
using backend.util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class ConfigUserController : ControllerBase
{
    private readonly DataContext _context;


    public ConfigUserController(DataContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<ConfigUser>> PostUser(CreateConfigUser user)
    {
        var newUser = new ConfigUser()
        {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            RoleId = user.RoleId
            
        };
        newUser.Password = AuthenticationUtils.HashPassword(user.Password);
        _context.ConfigUsers.Add(newUser);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetUser", new { id = newUser.Id }, user);
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ConfigUser>>> GetUsers()
    {
        return await _context.ConfigUsers.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ConfigUser>> GetUser(int id)
    {
        var user = await _context.ConfigUsers.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return user;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.ConfigUsers.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        _context.ConfigUsers.Remove(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, ConfigUser user)
    {
        if (id != user.Id)
        {
            return BadRequest();
        }

        user.Password = AuthenticationUtils.HashPassword(user.Password);
        _context.Entry(user).State = EntityState.Modified;

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

    [HttpPut("{id}/changepassword")]
    public async Task<IActionResult> ChangePassword(int id, string newPassword)
    {
        var user = await _context.ConfigUsers.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        user.Password = AuthenticationUtils.HashPassword(newPassword);
        _context.Entry(user).State = EntityState.Modified;

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
}