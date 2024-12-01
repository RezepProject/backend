using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Entities;
using Microsoft.IdentityModel.Tokens;

namespace backend.Util;

public static class AuthenticationUtils
{
    public static string GenerateJwtToken(Login login, int userId, int roleId)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("Jwt:Key") + ""));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            // save inside of token
            new Claim(ClaimTypes.NameIdentifier, login.UserIdentificator),
            new Claim(ClaimTypes.Role, roleId.ToString()),
            new Claim(ClaimTypes.Sid, userId.ToString())
        };

        var timeout = 15;
        if (Program.devMode) timeout = 4 * 60;

        var token = new JwtSecurityToken(Environment.GetEnvironmentVariable("Jwt:Issuer"),
            Environment.GetEnvironmentVariable("Jwt:Audience"),
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
        }
        catch
        {
            return false;
        }
    }
}