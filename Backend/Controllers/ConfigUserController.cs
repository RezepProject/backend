using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using backend.Entities;
using backend.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class ConfigUserController(DataContext ctx) : ControllerBase
{
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

        ctx.ConfigUserTokens.Add(userToken);
        await ctx.SaveChangesAsync();

        return MailUtil.SendMail(userToken.Email, "Test", CreateHtmlMailTemplate(userToken.Token))
            ? Ok()
            : StatusCode((int)HttpStatusCode.InternalServerError);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReturnConfigUser>>> GetUsers()
    {
        return (await ctx.ConfigUsers.ToListAsync()).Select(user => new ReturnConfigUser()
        {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Id = user.Id,
            RoleId = user.RoleId
        }).ToList();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReturnConfigUser>> GetUser(int id)
    {
        var user = await UserExists(id);
        return user == null
            ? NotFound("User not found")
            : new ReturnConfigUser()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                RoleId = user.RoleId
            };
    }

    [HttpGet("invitation")]
    public async Task<ActionResult<IEnumerable<ConfigUserToken>>> GetInvitations()
    {
        return await ctx.ConfigUserTokens.ToListAsync();
    }

    [HttpGet("invitation/{id}")]
    public async Task<ActionResult<ConfigUserToken>> GetInvitation(int id)
    {
        var user = await ctx.ConfigUserTokens.FirstOrDefaultAsync(u => u.Id == id);
        return user == null ? NotFound("User not found") : user;
    }

    [HttpDelete("invitation/{id}")]
    public async Task<IActionResult> DeleteInvitation(int id)
    {
        var user = await ctx.ConfigUserTokens.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            return NotFound("User not found");
        }

        ctx.ConfigUserTokens.Remove(user);
        await ctx.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("invitation/{id}/resend")]
    public async Task<IActionResult> ResendInvitation(int id)
    {
        var user = await ctx.ConfigUserTokens.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            return NotFound("User not found");
        }

        user.Token = Guid.NewGuid();
        user.CreatedAt = DateTime.Now.ToUniversalTime();
        ctx.ConfigUserTokens.Update(user);
        await ctx.SaveChangesAsync();

        return MailUtil.SendMail(user.Email, "Test", CreateHtmlMailTemplate(user.Token))
            ? Ok()
            : StatusCode((int)HttpStatusCode.InternalServerError);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await UserExists(id);
        if (user == null)
        {
            return NotFound("User not found");
        }

        ctx.ConfigUsers.Remove(user);
        await ctx.SaveChangesAsync();
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

        ctx.ConfigUsers.Update(userToUpdate);

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

    [HttpPut("{id}/change-password")]
    public async Task<IActionResult> ChangePassword(int id, [Required] string newPassword)
    {
        var user = await UserExists(id);
        if (user == null)
        {
            return NotFound("User not found");
        }

        user.Password = AuthenticationUtils.HashPassword(newPassword);
        ctx.ConfigUsers.Update(user);

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

    private async Task<ConfigUser?> UserExists(int id)
    {
        return await ctx.ConfigUsers.FindAsync(id);
    }

    private async Task<bool> EmailIsUsed(string email)
    {
        return await ctx.ConfigUsers.AnyAsync(user => user.Email == email) ||
               await ctx.ConfigUserTokens.AnyAsync(user => user.Email == email);
    }

    private static string CreateHtmlMailTemplate(Guid token)
    {
        string htmlContent = System.IO.File.ReadAllText("Resources/InviteMail.html");

        htmlContent = htmlContent.Replace("{GUID}", token.ToString());

        return htmlContent;
    }
}