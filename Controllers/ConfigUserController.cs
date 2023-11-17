using System.ComponentModel.DataAnnotations;
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
        if (await EmailIsUsed(user.Email))
        {
            return BadRequest("Email is already used");
        }

        var newUser = new ConfigUser
        {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            RoleId = user.RoleId,
            Password = AuthenticationUtils.HashPassword(user.Password)
        };

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
        var user = await UserExists(id);
        return user == null ? NotFound("User not found") : user;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await UserExists(id);
        if (user == null)
        {
            return NotFound("User not found");
        }

        _context.ConfigUsers.Remove(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, ChangeConfigUser user)
    {
        var userToUpdate = await UserExists(id);
        if (userToUpdate == null)
        {
            return NotFound("User not found");
        }

        if (await EmailIsUsed(user.Email))
        {
            return BadRequest("Email is already used");
        }

        userToUpdate.Email = user.Email;
        userToUpdate.FirstName = user.FirstName;
        userToUpdate.LastName = user.LastName;
        userToUpdate.RoleId = user.RoleId;

        _context.ConfigUsers.Update(userToUpdate);

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

    [HttpPut("{id}/change-password")]
    public async Task<IActionResult> ChangePassword(int id, [Required] string newPassword)
    {
        var user = await UserExists(id);
        if (user == null)
        {
            return NotFound("User not found");
        }

        user.Password = AuthenticationUtils.HashPassword(newPassword);
        _context.ConfigUsers.Update(user);

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

    private async Task<ConfigUser?> UserExists(int id)
    {
        return await _context.ConfigUsers.FindAsync(id);
    }

    private async Task<bool> EmailIsUsed(string email)
    {
        return await _context.ConfigUsers.AnyAsync(user => user.Email == email);
    }
}