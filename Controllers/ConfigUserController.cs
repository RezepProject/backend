using System.ComponentModel.DataAnnotations;
using System.Net;
using backend.Entities;
using backend.util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class ConfigUserController : ControllerBase
{
    private readonly DataContext _ctx;


    public ConfigUserController(DataContext ctx)
    {
        _ctx = ctx;
    }

    [HttpPost]
    public async Task<ActionResult> PostUser(CreateUserToken user)
    {
        if (await EmailIsUsed(user.Email))
        {
            return BadRequest("Email is already used!");
        }

        var userToken = new ConfigUserToken()
        {
            Email = user.Email,
            RoleId = user.RoleId,
            CreatedAt = DateTime.Now.ToUniversalTime(),
            Token = Guid.NewGuid()
        };

        _ctx.ConfigUserTokens.Add(userToken);
        await _ctx.SaveChangesAsync();

        return MailUtil.SendMail(userToken.Email, "Test", $"{userToken.Token}") ?
            Ok() : StatusCode((int) HttpStatusCode.InternalServerError);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ConfigUser>>> GetUsers()
    {
        return await _ctx.ConfigUsers.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReturnConfigUser>> GetUser(int id)
    {
        var user = await UserExists(id);
        return user == null ? NotFound("User not found") : new ReturnConfigUser()
        {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Id = user.Id,
            RoleId = user.RoleId
        };
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await UserExists(id);
        if (user == null)
        {
            return NotFound("User not found");
        }

        _ctx.ConfigUsers.Remove(user);
        await _ctx.SaveChangesAsync();
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

        _ctx.ConfigUsers.Update(userToUpdate);

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

    [HttpPut("{id}/change-password")]
    public async Task<IActionResult> ChangePassword(int id, [Required] string newPassword)
    {
        var user = await UserExists(id);
        if (user == null)
        {
            return NotFound("User not found");
        }

        user.Password = AuthenticationUtils.HashPassword(newPassword);
        _ctx.ConfigUsers.Update(user);

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

    private async Task<ConfigUser?> UserExists(int id)
    {
        return await _ctx.ConfigUsers.FindAsync(id);
    }

    private async Task<bool> EmailIsUsed(string email)
    {
        return await _ctx.ConfigUsers.AnyAsync(user => user.Email == email) ||
               await _ctx.ConfigUserTokens.AnyAsync(user => user.Email == email);
    }
}