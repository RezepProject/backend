using Microsoft.AspNetCore.Mvc;
using backend.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class ConfigController(DataContext ctx) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Config>>> GetConfigs()
    {
        return await ctx.Configs.ToListAsync();
    }
        
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Config>> GetConfig(int id)
    {
        var config = await ctx.Configs.FindAsync(id);

        if (config == null)
        {
            return NotFound("Config not found!");
        }

        return config;
    }
        
    [HttpPost]
    public async Task<ActionResult<Config>> AddConfig(CreateConfig config)
    {
        var newConfig = new Config
        {
            Title = config.Title,
            Value = config.Value
        };

        ctx.Configs.Add(newConfig);
        await ctx.SaveChangesAsync();

        return CreatedAtAction("GetConfig", new { id = newConfig.Id }, newConfig);
    }
        
    [HttpPut("{id:int}")]
    public async Task<IActionResult> ChangeConfig(int id, CreateConfig config)
    {
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
        
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteConfig(int id)
    {
        var config = await ctx.Configs.FindAsync(id);
        if (config == null)
        {
            return NotFound("Config not found!");
        }

        ctx.Configs.Remove(config);
        await ctx.SaveChangesAsync();

        return NoContent();
    }
}