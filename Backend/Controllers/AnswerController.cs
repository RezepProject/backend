using backend.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class AnswerController(DataContext ctx, IValidator<CreateAnswer> createValidator, IValidator<UpdateAnswer> updateValidator) : ControllerBase
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

        if (answer == null) return NotFound("Answer id not found!");

        return answer;
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> ChangeAnswer(int id, UpdateAnswer answer)
    {
        var validationResult = await updateValidator.ValidateAsync(answer);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var answerToUpdate = await ctx.Answers.FindAsync(id);
        if (answerToUpdate == null) return NotFound("Answer id not found!");

        answerToUpdate.Text = answer.Text;
        answerToUpdate.User = answer.User;

        ctx.Entry(answerToUpdate).State = EntityState.Modified;
        await ctx.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Answer>> AddAnswer(CreateAnswer answer)
    {
        var validationResult = await createValidator.ValidateAsync(answer);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var newAnswer = new Answer { Text = answer.Text, User = answer.User };
        ctx.Answers.Add(newAnswer);
        await ctx.SaveChangesAsync();

        return CreatedAtAction("GetAnswer", new { id = newAnswer.Id }, newAnswer);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAnswer(int id)
    {
        var answer = await ctx.Answers.FindAsync(id);
        if (answer == null) return NotFound("Answer id not found!");

        ctx.Answers.Remove(answer);
        await ctx.SaveChangesAsync();

        return NoContent();
    }
}
