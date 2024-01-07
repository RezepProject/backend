using System.Net;
using backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class QuestionController(DataContext ctx) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Question>>> GetQuestions()
    {
        return await ctx.Questions.ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Question>> GetQuestion(int id)
    {
        var question = await ctx.Questions.FindAsync(id);

        if (question == null)
        {
            return NotFound("Question id not found!");
        }

        return question;
    }

    [HttpPost]
    public async Task<ActionResult<Question>> AddQuestion(CreateQuestion question)
    {
        var questionEntity = new Question()
        {
            Text = question.Text,
            Answers = question.Answers?.Select(answer => new Answer()
            {
                Text = answer.Text,
                User = answer.User
            }).ToList()
        };
        
        ctx.Questions.Add(questionEntity);
        await ctx.SaveChangesAsync();

        return CreatedAtAction("GetQuestion", new { id = questionEntity.Id }, question);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateQuestion(int id, Question question)
    {
        if (id != question.Id)
        {
            return BadRequest("Question id not valid!");
        }

        ctx.Entry(question).State = EntityState.Modified;

        try
        {
            await ctx.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return QuestionExists(id) ? StatusCode((int) HttpStatusCode.InternalServerError) : NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteQuestion(int id)
    {
        var question = await ctx.Questions.FindAsync(id);
        if (question == null)
        {
            return NotFound("Question id not found!");
        }

        ctx.Questions.Remove(question);
        await ctx.SaveChangesAsync();

        return NoContent();
    }

    private bool QuestionExists(int id)
    {
        return ctx.Questions.Any(e => e.Id == id);
    }
}