using backend.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class BackgroundImageController(DataContext ctx) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BackgroundImage>>> GetBackgroundImages()
    {
        return await ctx.BackgroundImages.ToListAsync();
    }    
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<BackgroundImage>> GetBackgroundImages(int id)
    {
        var img = await ctx.BackgroundImages.FindAsync(id);
        return img == null ? NotFound("Image id not found!") : img;
    }
    
    
    
    [HttpPost]
    public async Task<ActionResult<BackgroundImage>> AddBackgroundImage(CreateBackgroundImage bi)
    {
        var backgroundImage = new BackgroundImage
        {
            Base64Image = bi.Base64Image
        };
        
        ctx.BackgroundImages.Add(backgroundImage);
        await ctx.SaveChangesAsync();

        return CreatedAtAction("GetBackgroundImages", new { id = backgroundImage.Id }, backgroundImage);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBackgroundImage(int id)
    {
        var bi = await ctx.BackgroundImages.FindAsync(id);
        if (bi == null) return NotFound("BackgroundImage id not found!");

        ctx.BackgroundImages.Remove(bi);
        await ctx.SaveChangesAsync();

        return NoContent();
    }
}