using System.Net;
using backend.Entities;
using backend.Util;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ConfigUserController : GenericController<ConfigUser, int>
    {
        private readonly IValidator<CreateUserToken> _createUserValidator;
        private readonly IValidator<ChangeConfigUser> _changeUserValidator;

        public ConfigUserController(DataContext ctx,
                                    IValidator<CreateUserToken> createUserValidator,
                                    IValidator<ChangeConfigUser> changeUserValidator)
            : base(ctx)
        {
            _createUserValidator = createUserValidator;
            _changeUserValidator = changeUserValidator;
        }

        [HttpPost]
        public async Task<ActionResult> PostUser(CreateUserToken user)
        {
            var validationResult = await _createUserValidator.ValidateAsync(user);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (await EmailIsUsed(user.Email))
                return BadRequest("Email is already used!");

            var userToken = new ConfigUserToken
            {
                Email = user.Email,
                RoleId = user.RoleId,
                CreatedAt = DateTime.UtcNow,
                Token = Guid.NewGuid()
            };

            ctx.ConfigUserTokens.Add(userToken);
            await ctx.SaveChangesAsync();

            return MailUtil.SendMail(userToken.Email, "Test", CreateHtmlMailTemplate(userToken.Token))
                ? Ok()
                : StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet("all-users")] // Renamed to avoid conflict
        public async Task<ActionResult<IEnumerable<ReturnConfigUser>>> GetUsers()
        {
            return (await ctx.ConfigUsers.AsNoTracking().ToListAsync()).Select(user => new ReturnConfigUser
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                RoleId = user.RoleId
            }).ToList();
        }

        [HttpGet("get-user/{id}")]
        public async Task<ActionResult<ReturnConfigUser>> GetUser(int id)
        {
            var user = await ctx.ConfigUsers.FindAsync(id);
            return user == null
                ? NotFound("User not found")
                : new ReturnConfigUser
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Id = user.Id,
                    RoleId = user.RoleId
                };
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, ChangeConfigUser user)
        {
            var validationResult = await _changeUserValidator.ValidateAsync(user);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var userToUpdate = await ctx.ConfigUsers.FindAsync(id);
            if (userToUpdate == null)
                return NotFound("User not found");

            if (await EmailIsUsed(user.Email))
                return BadRequest("Email is already used");

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
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

            return NoContent();
        }

        private async Task<bool> EmailIsUsed(string email)
        {
            return await ctx.ConfigUsers.AnyAsync(user => user.Email == email) ||
                   await ctx.ConfigUserTokens.AnyAsync(user => user.Email == email);
        }

        private static string CreateHtmlMailTemplate(Guid token)
        {
            var htmlContent = System.IO.File.ReadAllText("Resources/InviteMail.html");
            htmlContent = htmlContent.Replace("{GUID}", token.ToString());
            return htmlContent;
        }
    }
}
