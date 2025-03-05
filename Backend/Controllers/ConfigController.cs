using System.Net;
using backend.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ConfigController : GenericController<Config, int>
    {
        private readonly IValidator<CreateConfig> _configValidator;

        public ConfigController(DataContext ctx, IValidator<CreateConfig> configValidator)
            : base(ctx)
        {
            _configValidator = configValidator;
        }

        [HttpPost]
        public async Task<ActionResult<Config>> AddConfig([FromBody] CreateConfig config)
        {
            // Validate the CreateConfig model
            var validationResult = await _configValidator.ValidateAsync(config);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors); // Return 400 with validation errors
            }

            var newConfig = new Config
            {
                Title = config.Title,
                Value = config.Value
            };

            ctx.Configs.Add(newConfig);
            await ctx.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEntity), new { id = newConfig.Id }, newConfig);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> ChangeConfig(int id, [FromBody] CreateConfig config)
        {
            // Validate the CreateConfig model
            var validationResult = await _configValidator.ValidateAsync(config);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors); // Return 400 with validation errors
            }

            var configToUpdate = await ctx.Configs.FindAsync(id);

            if (configToUpdate == null)
            {
                return NotFound("Config not found!");
            }

            configToUpdate.Title = config.Title;
            configToUpdate.Value = config.Value;

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
    }
}