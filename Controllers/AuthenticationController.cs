using backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.util;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly DataContext _ctx;

        public AuthenticationController(DataContext ctx)
        {
            _ctx = ctx;
        }

        [HttpPost(Name = "Authorize")]
        public async Task<ActionResult<string>> Login([FromBody] Login login)
        {
            // login with username or email
            /* var user = await _ctx.ConfigUsers.FirstOrDefaultAsync(u => u.Id.ToString() == login.UserIdentificator);
            if (user == null) user = await _ctx.ConfigUsers.FirstOrDefaultAsync(u => u.Email == login.UserIdentificator); */

            var user = await _ctx.ConfigUsers
                           .FirstOrDefaultAsync(u => u.Id.ToString() == login.UserIdentificator) ??
                       await _ctx.ConfigUsers.FirstOrDefaultAsync(u => u.Email == login.UserIdentificator);

            if (user == null || !AuthenticationUtils.VerifyPassword(login.Password, user.Password))
            {
                return Unauthorized();
            }

            return Ok(AuthenticationUtils.GenerateJwtToken(login, user.Id, user.RoleId));
        }

        [HttpPost("applytoken")]
        public async Task<ActionResult> ApplyToken(CreateConfigUser user)
        {
            var token = await _ctx.ConfigUserTokens.FirstOrDefaultAsync(token => token.Token == user.Token);
            if (token == null)
            {
                return NotFound("Token not found");
            }

            if(token.CreatedAt.ToUniversalTime().AddDays(1) < DateTime.Now.ToUniversalTime())
            {
                return BadRequest("Token expired");
            }

            var newUser = new ConfigUser
            {
                Email = token.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RoleId = token.RoleId,
                Password = AuthenticationUtils.HashPassword(user.Password)
            };

            _ctx.ConfigUsers.Add(newUser);
            _ctx.ConfigUserTokens.Remove(token);
            await _ctx.SaveChangesAsync();

            var locationUri = Url.Link("GetUser", new { controller = "ConfigUser", id = newUser.Id });
            return CreatedAtAction(locationUri, new { id = newUser.Id }, newUser);
        }
    }
}