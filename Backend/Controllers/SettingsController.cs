using backend.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class SettingsController(DataContext ctx) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Setting>>> GetSettings()
    {
        return await ctx.Settings.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Setting>> GetSetting(string id)
    {
        var setting = await ctx.Settings.FindAsync(id);
        return setting == null ? NotFound("Setting id not found!") : setting;
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateSetting(int id, CreateSetting newSetting)
    {
        var setting = await ctx.Settings.FindAsync(id);
        if (setting == null) return NotFound("Setting id not found!");

        setting.Name = newSetting.Name;
        setting.BackgroundImage = newSetting.BackgroundImage;
        setting.Language = newSetting.Language;
        setting.TalkingSpeed = newSetting.TalkingSpeed;
        setting.GreetingMessage = newSetting.GreetingMessage;
        setting.State = newSetting.State;
        ctx.Settings.Update(setting);

        try
        {
            await ctx.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Setting>> CreateSetting(CreateSetting setting)
    {
        var newSetting = ctx.Settings.Add(new Setting
        {
            Name = setting.Name,
            BackgroundImage = setting.BackgroundImage,
            Language = setting.Language,
            TalkingSpeed = setting.TalkingSpeed,
            GreetingMessage = setting.GreetingMessage,
            State = setting.State
        });
        await ctx.SaveChangesAsync();

        return CreatedAtAction("GetSetting", new { id = newSetting.Entity.Id }, setting);
    }
}