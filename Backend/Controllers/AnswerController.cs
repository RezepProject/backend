using backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class AnswerController(DataContext ctx) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Answer>>> GetAnswers()
    {
        return await ctx.Answers.ToListAsync();
    }
        
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Answer>> GetAnswer(int id)
    {
        var answer = await ctx.Answers.FindAsync(id);

        if (answer == null)
        {
            return NotFound("Answer id not found!");
        }

        return answer;
    }
        
    [HttpPut("{id:int}")]
    public async Task<IActionResult> ChangeAnswer(int id, UpdateAnswer answer)
    {
        var answerToUpdate = await ctx.Answers.FindAsync(id);

        if (answerToUpdate == null)
        {
            return NotFound("Answer id not found!");
        }
        
        answerToUpdate.Text = answer.Text;

        ctx.Entry(answerToUpdate).State = EntityState.Modified;

        await ctx.SaveChangesAsync();
        return NoContent();
    }
        
    [HttpPost]
    public async Task<ActionResult<Answer>> AddAnswer(Answer answer)
    {
        ctx.Answers.Add(answer);
        await ctx.SaveChangesAsync();

        return CreatedAtAction("GetAnswer", new { id = answer.Id }, answer);
    }
        
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAnswer(int id)
    {
        var answer = await ctx.Answers.FindAsync(id);
        if (answer == null)
        {
            return NotFound("Answer id not found!");
        }

        ctx.Answers.Remove(answer);
        await ctx.SaveChangesAsync();

        return NoContent();
    }
}