using backend.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class BackgroundImageController : GenericController<BackgroundImage, int>
    {
        private readonly IValidator<CreateBackgroundImage> _validator;

        public BackgroundImageController(DataContext context, IValidator<CreateBackgroundImage> validator)
            : base(context)
        {
            _validator = validator;
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(BackgroundImage), 201)]
        [ProducesResponseType(typeof(IEnumerable<string>), 400)]
        public async Task<ActionResult<BackgroundImage>> AddBackgroundImage([FromBody] CreateBackgroundImage bi)
        {
            var validationResult = await _validator.ValidateAsync(bi);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var backgroundImage = new BackgroundImage
            {
                Base64Image = bi.Base64Image
            };

            ctx.BackgroundImages.Add(backgroundImage);
            await ctx.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEntity), new { id = backgroundImage.Id }, backgroundImage);
        }
    }
}