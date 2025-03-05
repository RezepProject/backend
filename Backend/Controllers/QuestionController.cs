using System.Net;
using backend.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class QuestionController : GenericController<Question, int>
    {
        private readonly IValidator<CreateQuestion> _createQuestionValidator;

        public QuestionController(DataContext ctx, IValidator<CreateQuestion> createQuestionValidator)
            : base(ctx)
        {
            _createQuestionValidator = createQuestionValidator;
        }

        [HttpGet("detailed")]
        public async Task<ActionResult<IEnumerable<object>>> GetQuestions()
        {
            var questions = await ctx.Questions
                .Include(q => q.Answers)
                .Include(q => q.Categories)
                .Select(q => new
                {
                    q.Id,
                    q.Text,
                    Categories = q.Categories.Select(c => new
                    {
                        c.Name,
                        c.Id
                    }).ToList(),
                    Answers = q.Answers.Select(a => new
                    {
                        a.Id,
                        a.Text,
                        a.User
                    }).ToList()
                })
                .ToListAsync();

            return Ok(questions);
        }

        [HttpPost("add")]
        public async Task<ActionResult<object>> AddQuestion(CreateQuestion question)
        {
            var validationResult = await _createQuestionValidator.ValidateAsync(question);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            List<QuestionCategory> categories = new();

            foreach (var category in question.Categories)
            {
                var categoryEntity = await ctx.QuestionCategories
                    .FirstOrDefaultAsync(c => c.Name == category.Name);
                if (categoryEntity == null)
                {
                    categoryEntity = new QuestionCategory
                    {
                        Name = category.Name
                    };
                    ctx.QuestionCategories.Add(categoryEntity);
                    await ctx.SaveChangesAsync();
                }

                categories.Add(categoryEntity);
            }

            var questionEntity = new Question
            {
                Text = question.Text,
                Categories = categories,
                Answers = question.Answers?.Select(answer => new Answer
                {
                    Text = answer.Text,
                    User = answer.User
                }).ToList()
            };

            ctx.Questions.Add(questionEntity);
            await ctx.SaveChangesAsync();

            // Return a simplified object to avoid circular references
            var result = new
            {
                questionEntity.Id,
                questionEntity.Text,
                Categories = questionEntity.Categories.Select(c => new
                {
                    c.Id,
                    c.Name
                }),
                Answers = questionEntity.Answers?.Select(a => new
                {
                    a.Id,
                    a.Text,
                    a.User
                })
            };

            return CreatedAtAction(nameof(GetEntity), new { id = questionEntity.Id }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateQuestion(int id, CreateQuestion question)
        {
            var validationResult = await _createQuestionValidator.ValidateAsync(question);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var questionEntity = await ctx.Questions
                .Include(q => q.Answers)
                .Include(q => q.Categories)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (questionEntity == null) return NotFound("Question id not found!");

            questionEntity.Text = question.Text;

            var allCategories = await ctx.QuestionCategories.ToListAsync();
            questionEntity.Categories = allCategories
                .Where(c => question.Categories.Any(qc => qc.Name == c.Name))
                .ToList();

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

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<QuestionCategory>>> GetCategories()
        {
            var categories = await ctx.QuestionCategories.ToListAsync();
            return Ok(categories);
        }
        
        [HttpDelete("{id}")]
        public override async Task<IActionResult> DeleteEntity(int id)
        {
            var question = await ctx.Questions
                .Include(q => q.Answers)
                .Include(q => q.Categories)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null)
                return NotFound($"{typeof(Question).Name} not found!");

            // Remove associated answers
            if (question.Answers != null && question.Answers.Any())
            {
                ctx.Answers.RemoveRange(question.Answers);
            }

            // Remove associations with categories
            if (question.Categories != null && question.Categories.Any())
            {
                question.Categories.Clear();
            }

            ctx.Questions.Remove(question);
            await ctx.SaveChangesAsync();

            return NoContent();
        }
    }
}