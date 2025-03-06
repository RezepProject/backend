using backend.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class AnswerController : GenericController<Answer, int>
    {
        private readonly IValidator<CreateAnswer> _createValidator;
        private readonly IValidator<UpdateAnswer> _updateValidator;

        public AnswerController(DataContext context, IValidator<CreateAnswer> createValidator, IValidator<UpdateAnswer> updateValidator)
            : base(context)
        {
            this._createValidator = createValidator;
            this._updateValidator = updateValidator;
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Answer), 201)]
        [ProducesResponseType(typeof(IEnumerable<string>), 400)]
        public async Task<ActionResult<Answer>> AddAnswer([FromBody] CreateAnswer answer)
        {
            var validationResult = await _createValidator.ValidateAsync(answer);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            var newAnswer = new Answer { Text = answer.Text, User = answer.User };
            ctx.Answers.Add(newAnswer);
            await ctx.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEntity), new { id = newAnswer.Id }, newAnswer);
        }

        [HttpPut("{id:int}")]
        [Consumes("application/json")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(IEnumerable<string>), 400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ChangeAnswer(int id, [FromBody] UpdateAnswer answer)
        {
            var validationResult = await _updateValidator.ValidateAsync(answer);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            var answerToUpdate = await ctx.Answers.FindAsync(id);
            if (answerToUpdate == null) return NotFound("Answer id not found!");

            answerToUpdate.Text = answer.Text;
            answerToUpdate.User = answer.User;

            ctx.Entry(answerToUpdate).State = EntityState.Modified;
            await ctx.SaveChangesAsync();
            return NoContent();
        }
    }
}
