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
        private readonly ILogger<AuthenticationController> _logger;

        private readonly DataContext _ctx;

        public AuthenticationController(DataContext ctx, ILogger<AuthenticationController> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        [HttpPost(Name = "Authorize")]
        public async Task<ActionResult<string>> Login([FromBody] Login login)
        {
            // login with username or email
            /* var user = await _ctx.ConfigUsers.FirstOrDefaultAsync(u => u.Id.ToString() == login.UserIdentificator);
            if (user == null) user = await _ctx.ConfigUsers.FirstOrDefaultAsync(u => u.Email == login.UserIdentificator); */

            var user = await _ctx.ConfigUsers.FirstOrDefaultAsync(u => u.Id.ToString() == login.UserIdentificator) ??
                       await _ctx.ConfigUsers.FirstOrDefaultAsync(u => u.Email == login.UserIdentificator);

            if(user == null || !AuthenticationUtils.VerifyPassword(login.Password, user.Password)) return Unauthorized();

            return Ok(AuthenticationUtils.GenerateJwtToken(login, user.Id));
        }
    }
}