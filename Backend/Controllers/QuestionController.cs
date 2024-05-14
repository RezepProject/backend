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
        return await ctx.Questions
            .Include(q => q.Answers)
            .ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Question>> GetQuestion(int id)
    {
        var question = await ctx.Questions.FindAsync(id);

        if (question == null) return NotFound("Question id not found!");

        return question;
    }

    [HttpPost]
    public async Task<ActionResult<Question>> AddQuestion(CreateQuestion question)
    {
        var questionEntity = new Question
        {
            Text = question.Text,
            Answers = question.Answers?.Select(answer => new Answer
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
    public async Task<IActionResult> UpdateQuestion(int id, CreateQuestion question)
    {
        var questionEntity = await ctx.Questions
            .Include(q => q.Answers)
            .FirstOrDefaultAsync(q => q.Id == id);

        if (questionEntity == null) return NotFound("Question id not found!");

        questionEntity.Text = question.Text;

        if (questionEntity.Answers != null)
        {
            ctx.Answers.RemoveRange(questionEntity.Answers);
            questionEntity.Answers.Clear();
        }

        questionEntity.Answers = question.Answers?.Select(answer => new Answer
        {
            Text = answer.Text,
            User = answer.User
        }).ToList();

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
    public async Task<IActionResult> DeleteQuestion(int id)
    {
        var question = await ctx.Questions
            .Include(q => q.Answers)
            .FirstOrDefaultAsync(q => q.Id == id);
        if (question == null) return NotFound("Question id not found!");

        if (question.Answers != null) ctx.Answers.RemoveRange(question.Answers);

        ctx.Questions.Remove(question);
        await ctx.SaveChangesAsync();

        return NoContent();
    }

    private bool QuestionExists(int id)
    {
        return ctx.Questions.Any(e => e.Id == id);
    }
}