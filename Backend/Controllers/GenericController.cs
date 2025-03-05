using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public abstract class GenericController<TEntity, TKey> : ControllerBase where TEntity : class
    {
        protected readonly DataContext ctx;

        public GenericController(DataContext context)
        {
            ctx = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TEntity>>> GetEntities()
        {
            return await ctx.Set<TEntity>().ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TEntity>> GetEntity(TKey id)
        {
            var entity = await ctx.Set<TEntity>().FindAsync(id);

            if (entity == null) return NotFound($"{typeof(TEntity).Name} not found!");

            return entity;
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> DeleteEntity(TKey id)
        {
            var entity = await ctx.Set<TEntity>().FindAsync(id);
            if (entity == null) return NotFound($"{typeof(TEntity).Name} not found!");

            ctx.Set<TEntity>().Remove(entity);
            await ctx.SaveChangesAsync();

            return NoContent();
        }
    }
}