using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
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

        return MailUtil.SendMail(userToken.Email, "Test", CreateHtmlMailTemplate(userToken.Token)) ?
            Ok() : StatusCode((int) HttpStatusCode.InternalServerError);
    }

    private static string CreateHtmlMailTemplate(Guid token)
    {
        var sb = new StringBuilder();
        sb.Append("<html>");
        sb.Append("<head>");
        sb.Append("<style>");
        sb.Append("body {font-family: Arial, sans-serif;}");
        sb.Append("h1 {color: #333;}");
        sb.Append("p {font-size: 14px;}");
        sb.Append(".button {background-color: #4CAF50; border: none; color: white; padding: 15px 32px; text-align: center; text-decoration: none; display: inline-block; font-size: 16px; margin: 4px 2px; cursor: pointer;}");
        sb.Append("</style>");
        sb.Append("</head>");
        sb.Append("<body>");
        sb.Append("<h1>Welcome to REZEP</h1>");
        sb.Append("<p>Please click this button to register:</p>");
        sb.Append($"<a href='http://localhost:5001/register/{token}' class='button'>Register</a>");
        sb.Append("</body>");
        sb.Append("</html>");

        return sb.ToString();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReturnConfigUser>>> GetUsers()
    {
        return (await _ctx.ConfigUsers.ToListAsync()).Select(user => new ReturnConfigUser()
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
        return user == null ? NotFound("User not found") : new ReturnConfigUser()
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
        return await _ctx.ConfigUserTokens.ToListAsync();
    }

    [HttpGet("invitation/{token}")]
    public async Task<ActionResult<ConfigUserToken>> GetInvitation(Guid token)
    {
        var user = await _ctx.ConfigUserTokens.FirstOrDefaultAsync(u => u.Token == token);
        return user == null ? NotFound("User not found") : user;
    }

    [HttpDelete("invitation/{token}")]
    public async Task<IActionResult> DeleteInvitation(Guid token)
    {
        var user = await _ctx.ConfigUserTokens.FirstOrDefaultAsync(u => u.Token == token);
        if (user == null)
        {
            return NotFound("User not found");
        }

        _ctx.ConfigUserTokens.Remove(user);
        await _ctx.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("invitation/{token}/resend")]
    public async Task<IActionResult> ResendInvitation(Guid token)
    {
        var user = await _ctx.ConfigUserTokens.FirstOrDefaultAsync(u => u.Token == token);
        if (user == null)
        {
            return NotFound("User not found");
        }

        user.Token = Guid.NewGuid();
        user.CreatedAt = DateTime.Now.ToUniversalTime();
        _ctx.ConfigUserTokens.Update(user);
        await _ctx.SaveChangesAsync();

        return MailUtil.SendMail(user.Email, "Test", CreateHtmlMailTemplate(user.Token)) ?
            Ok() : StatusCode((int) HttpStatusCode.InternalServerError);
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