using System.Security.Cryptography;
using backend.Entities;
using backend.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController(DataContext ctx) : ControllerBase
{
    [HttpPost(Name = "Authorize")]
    public async Task<ActionResult<string>> Login([FromBody] Login login)
    {
        // login with username or email
        /* var user = await _ctx.ConfigUsers.FirstOrDefaultAsync(u => u.Id.ToString() == login.UserIdentificator);
        if (user == null) user = await _ctx.ConfigUsers.FirstOrDefaultAsync(u => u.Email == login.UserIdentificator); */

        var user = await ctx.ConfigUsers
                       .FirstOrDefaultAsync(u => u.Email == login.UserIdentificator)
                   ?? await ctx.ConfigUsers
                       .FirstOrDefaultAsync(u => u.Id.ToString() == login.UserIdentificator);

        if (user == null || !AuthenticationUtils.VerifyPassword(login.Password, user.Password))
        {
            await Task.Delay(3000);
            return Unauthorized();
        }

        var refreshToken = GenerateRefreshToken();
        await SetRefreshToken(refreshToken, user);

        return Ok(AuthenticationUtils.GenerateJwtToken(login, user.Id, user.RoleId));
    }

    [HttpPost("applytoken")]
    public async Task<ActionResult> ApplyToken(CreateConfigUser user)
    {
        var token = await ctx.ConfigUserTokens.FirstOrDefaultAsync(token => token.Token == user.Token);
        if (token == null) return NotFound("Token not found");

        if (token.CreatedAt.ToUniversalTime().AddDays(1) < DateTime.Now.ToUniversalTime())
            return BadRequest("Token expired");

        var newUser = new ConfigUser
        {
            Email = token.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            RoleId = token.RoleId,
            Password = AuthenticationUtils.HashPassword(user.Password)
        };

        ctx.ConfigUsers.Add(newUser);
        ctx.ConfigUserTokens.Remove(token);
        await ctx.SaveChangesAsync();

        var locationUri = Url.Link("GetUser", new { controller = "ConfigUser", id = newUser.Id });
        return CreatedAtAction(locationUri, new { id = newUser.Id }, newUser);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<string>> RefreshToken(int userId)
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var user = await ctx.ConfigUsers.FindAsync(userId);

        if (user == null) return BadRequest("User not found.");


        if (!user.RefreshToken.Equals(refreshToken)) return Unauthorized("Invalid Refresh Token.");

        if (user.TokenExpires < DateTime.Now) return Unauthorized("Token expired.");

        var login = new Login
        {
            Password = user.Password,
            UserIdentificator = user.Email
        };

        var token = AuthenticationUtils.GenerateJwtToken(login, user.Id, user.RoleId);
        var newRefreshToken = GenerateRefreshToken();
        await SetRefreshToken(newRefreshToken, user);

        return Ok(token);
    }

    private RefreshToken GenerateRefreshToken()
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.Now.AddDays(7),
            Created = DateTime.Now
        };

        return refreshToken;
    }

    private async Task SetRefreshToken(RefreshToken newRefreshToken, ConfigUser user)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = newRefreshToken.Expires
        };
        Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

        user.RefreshToken = newRefreshToken.Token;
        user.TokenCreated = newRefreshToken.Created;
        user.TokenExpires = newRefreshToken.Expires;
        ctx.RefreshTokens.Add(newRefreshToken);
        ctx.ConfigUsers.Update(user);
        await ctx.SaveChangesAsync();
    }
}