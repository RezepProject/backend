using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace backend.Util;

public static class AuthenticationUtils
{
    public static string GenerateJwtToken(Login login, int userId, int roleId)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Program.config["Jwt:Key"] + ""));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            // save inside of token
            new Claim(ClaimTypes.NameIdentifier,login.UserIdentificator),
            new Claim(ClaimTypes.Role, roleId.ToString()),
            new Claim(ClaimTypes.Sid, userId.ToString()),
        };

        var timeout = 15;
        if(Program.devMode) timeout = 4 * 60;

        var token = new JwtSecurityToken(Program.config["Jwt:Issuer"],
            Program.config["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(timeout),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static JwtSecurityToken DecodeToken(string jwt)
    {
        var handler = new JwtSecurityTokenHandler();
        return handler.ReadJwtToken(jwt);
    }

    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        try
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        } catch { return false; }
    }

    public static async Task<int> AuthorizeUser(WebApplication app, HttpContext context)
    {
        try
        {
            var routeSplit = context.GetEndpoint()?.DisplayName?.Split('.');
            var route = routeSplit?[2].Replace("Controller", "") + "." + routeSplit?[3].Split(" ")[0];

            // always allowed endpoint
            var endpoint = route.Split(".")[0].ToLower();
            if (endpoint == "authentication") return 301;

            // Permission integration
            // var permissions = new List<Permission>();
            var dataContext = app.Services.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
            var tmpToken = context.Request.Headers.Authorization.ToString();

            if (tmpToken.IsNullOrEmpty() || tmpToken.ToLower().Trim() == "bearer") return 401;
            if (!tmpToken.ToLower().Contains("bearer")) tmpToken = "Bearer " + tmpToken;

            var token = DecodeToken(tmpToken.Split(" ")[1]);
            var userId = token.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

            if (token.ValidTo < DateTime.UtcNow) return 401;

            var rank = (await dataContext.ConfigUsers.Include(u => u.Role).FirstAsync(u => u.Id.ToString() == userId))
                    .Role;

            // Permission integration
            /* var perms = await dataContext.Permissions.Where(p => p.Ranks.Select(r => r.RankId).Contains(rank.RankId))
                .ToListAsync(); */
            /* foreach (var p in perms)
            {
                // * = every permission
                if (p.Name == "*") return 301;
                permissions.Add(p);
            }
            if (!permissions.Select(p => p.Name).Contains(route)) return 401; */

            return rank == null ? 401 : 301;
        }
        catch { return 500; }
    }
}