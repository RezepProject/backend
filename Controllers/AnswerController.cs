using System.Net;
using backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnswerController : ControllerBase
    {
        private readonly DataContext _ctx;

        public AnswerController(DataContext ctx)
        {
            _ctx = ctx;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Answer>>> GetAnswers()
        {
            return await _ctx.Answers.ToListAsync();
        }
        
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Answer>> GetAnswer(int id)
        {
            var answer = await _ctx.Answers.FindAsync(id);

            if (answer == null)
            {
                return NotFound("Answer id not found!");
            }

            return answer;
        }
        
        [HttpPut("{id:int}")]
        public async Task<IActionResult> ChangeAnswer(int id, UpdateAnswer answer)
        {
            var answerToUpdate = await _ctx.Answers.FindAsync(id);

            if (answerToUpdate == null)
            {
                return NotFound("Answer id not found!");
            }

            if (answer.QuestionId != null)
            {
                answerToUpdate.QuestionId = (int)answer.QuestionId;
            }
            answerToUpdate.Text = answer.Text;

            _ctx.Entry(answerToUpdate).State = EntityState.Modified;

            try
            {
                await _ctx.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

            return NoContent();
        }
        
        [HttpPost]
        public async Task<ActionResult<Answer>> AddAnswer(Answer answer)
        {
            _ctx.Answers.Add(answer);
            await _ctx.SaveChangesAsync();

            return CreatedAtAction("GetAnswer", new { id = answer.Id }, answer);
        }
        
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAnswer(int id)
        {
            var answer = await _ctx.Answers.FindAsync(id);
            if (answer == null)
            {
                return NotFound("Answer id not found!");
            }

            _ctx.Answers.Remove(answer);
            await _ctx.SaveChangesAsync();

            return NoContent();
        }
    }
}