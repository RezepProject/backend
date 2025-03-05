using backend.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class BackgroundImageController(DataContext ctx, IValidator<CreateBackgroundImage> validator) : ControllerBase
{
    private readonly IValidator<CreateBackgroundImage> _validator = validator;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BackgroundImage>>> GetBackgroundImages()
    {
        var images = await ctx.BackgroundImages.ToListAsync();
        return Ok(images);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BackgroundImage>> GetBackgroundImage(int id)
    {
        var img = await ctx.BackgroundImages.FindAsync(id);
        return img == null ? NotFound(new { message = "Image ID not found!" }) : Ok(img);
    }

    [HttpPost]
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

        return CreatedAtAction(nameof(GetBackgroundImage), new { id = backgroundImage.Id }, backgroundImage);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBackgroundImage(int id)
    {
        var bi = await ctx.BackgroundImages.FindAsync(id);
        if (bi == null)
            return NotFound(new { message = "BackgroundImage ID not found!" });

        ctx.BackgroundImages.Remove(bi);
        await ctx.SaveChangesAsync();

        return NoContent();
    }
}