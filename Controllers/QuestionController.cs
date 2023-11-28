using System.Net;
using backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly DataContext _ctx;

        public QuestionController(DataContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestions()
        {
            return await _ctx.Questions.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Question>> GetQuestion(int id)
        {
            var question = await _ctx.Questions.FindAsync(id);

            if (question == null)
            {
                return NotFound("Question id not found!");
            }

            return question;
        }

        [HttpPost]
        public async Task<ActionResult<Question>> AddQuestion(Question question)
        {
            _ctx.Questions.Add(question);
            await _ctx.SaveChangesAsync();

            return CreatedAtAction("GetQuestion", new { id = question.Id }, question);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateQuestion(int id, Question question)
        {
            if (id != question.Id)
            {
                return BadRequest("Question id not valid!");
            }

            _ctx.Entry(question).State = EntityState.Modified;

            try
            {
                await _ctx.SaveChangesAsync();
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
            var question = await _ctx.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound("Question id not found!");
            }

            _ctx.Questions.Remove(question);
            await _ctx.SaveChangesAsync();

            return NoContent();
        }

        private bool QuestionExists(int id)
        {
            return _ctx.Questions.Any(e => e.Id == id);
        }
    }
}