using backend.Entities;
using backend.Controllers.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class TaskController : ControllerBase
{
    private readonly DataContext ctx;

    public TaskController(DataContext ctx)
    {
        this.ctx = ctx;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EntityTask>>> GetTasks()
    {
        return await ctx.Tasks.ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EntityTask>> GetTask(int id)
    {
        var task = await ctx.Tasks.FindAsync(id);
        return task == null ? NotFound("Task id not found!") : task;
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTask(int id, UpdateTask updatedTask)
    {
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
    public async Task<ActionResult<EntityTask>> CreateTask(CreateTask newTask)
    {
        var validator = new CreateTaskValidator();
        var validationResult = await validator.ValidateAsync(newTask);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var task = ctx.Tasks.Add(new EntityTask { Text = newTask.Text, Done = newTask.Done });
        await ctx.SaveChangesAsync();

        return CreatedAtAction("GetTask", new { id = task.Entity.Id }, newTask);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await ctx.Tasks.FindAsync(id);
        if (task == null) return NotFound("Task id not found!");

        ctx.Tasks.Remove(task);
        await ctx.SaveChangesAsync();
        return NoContent();
    }
}
