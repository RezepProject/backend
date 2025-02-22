using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Entities;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class GenericController<TEntity, TCreateDto, TUpdateDto> : ControllerBase 
        where TEntity : class, IEntity
    {
        protected readonly DataContext _context;

        public GenericController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TEntity>>> GetAll()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TEntity>> GetById(int id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            return entity == null ? NotFound("Entity id not found!") : entity;
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity == null) return NotFound("Entity id not found!");

            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

