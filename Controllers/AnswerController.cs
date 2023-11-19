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
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Answer>> GetAnswer(int id)
        {
            var answer = await _ctx.Answers.FindAsync(id);

            if (answer == null)
            {
                return NotFound();
            }

            return answer;
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnswer(int id, Answer answer)
        {
            if (id != answer.Id)
            {
                return BadRequest();
            }

            _ctx.Entry(answer).State = EntityState.Modified;
            await _ctx.SaveChangesAsync();

            return NoContent();
        }
        
        [HttpPost]
        public async Task<ActionResult<Answer>> PostAnswer(Answer answer)
        {
            _ctx.Answers.Add(answer);
            await _ctx.SaveChangesAsync();

            return CreatedAtAction("GetAnswer", new { id = answer.Id }, answer);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnswer(int id)
        {
            var answer = await _ctx.Answers.FindAsync(id);
            if (answer == null)
            {
                return NotFound();
            }

            _ctx.Answers.Remove(answer);
            await _ctx.SaveChangesAsync();

            return NoContent();
        }
    }
}