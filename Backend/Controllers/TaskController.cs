using backend.Entities;
using backend.Controllers.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class TaskController : GenericController<EntityTask, int>
    {
        public TaskController(DataContext ctx) : base(ctx)
        {
        }

        [HttpPut("{id:int}")]
        [Consumes("application/json")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(IEnumerable<string>), 400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTask updatedTask)
        {
            // Use the UpdateTaskValidator
            var validator = new UpdateTaskValidator();
            var validationResult = await validator.ValidateAsync(updatedTask);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var task = await ctx.Tasks.FindAsync(id);
            if (task == null) return NotFound("Task id not found!");

            task.Text = updatedTask.Text;
            task.Done = updatedTask.Done;
            ctx.Tasks.Update(task);

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
        [Consumes("application/json")]
        [ProducesResponseType(typeof(EntityTask), 201)]
        [ProducesResponseType(typeof(IEnumerable<string>), 400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<EntityTask>> CreateTask([FromBody] CreateTask newTask)
        {
            // Use the CreateTaskValidator
            var validator = new CreateTaskValidator();
            var validationResult = await validator.ValidateAsync(newTask);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var task = ctx.Tasks.Add(new EntityTask { Text = newTask.Text, Done = newTask.Done });
            await ctx.SaveChangesAsync();

            return CreatedAtAction("GetEntity", new { id = task.Entity.Id }, newTask);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public override async Task<IActionResult> DeleteEntity(int id)
        {
            return await base.DeleteEntity(id);
        }
    }
}
