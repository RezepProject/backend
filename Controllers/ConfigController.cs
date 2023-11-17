using Microsoft.AspNetCore.Mvc;
using backend.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly DataContext _ctx;

        public ConfigController(DataContext ctx)
        {
            _ctx = ctx;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Config>>> GetConfigs()
        {
            return await _ctx.Configs.ToListAsync();
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Config>> GetConfig(int id)
        {
            var config = await _ctx.Configs.FindAsync(id);

            if (config == null)
            {
                return NotFound("Config not found");
            }

            return config;
        }
        
        [HttpPost]
        public async Task<ActionResult<Config>> PostConfig(CreateConfig config)
        {
            var newConfig = new Config
            {
                Title = config.Title,
                Value = config.Value
            };

            _ctx.Configs.Add(newConfig);
            await _ctx.SaveChangesAsync();

            return CreatedAtAction("GetConfig", new { id = newConfig.Id }, newConfig);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConfig(int id, CreateConfig config)
        {
            var configToUpdate = await _ctx.Configs.FindAsync(id);

            if (configToUpdate == null)
            {
                return NotFound("Config not found");
            }

            configToUpdate.Title = config.Title;
            configToUpdate.Value = config.Value;

            try
            {
                await _ctx.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConfigExists(id))
                {
                    return NotFound("Config not found");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfig(int id)
        {
            var config = await _ctx.Configs.FindAsync(id);
            if (config == null)
            {
                return NotFound();
            }

            _ctx.Configs.Remove(config);
            await _ctx.SaveChangesAsync();

            return NoContent();
        }

        private bool ConfigExists(int id)
        {
            return _ctx.Configs.Any(e => e.Id == id);
        }
    }
}