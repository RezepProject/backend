using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using backend;
using backend.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Test.Utils;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace Test.Controller
{
    [Collection("Sequential Tests")]
    public class QuestionControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public QuestionControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            factory.InitializeDatabase();
            SeedDataAsync(factory).GetAwaiter().GetResult();
        }

        private async Task SeedDataAsync(CustomWebApplicationFactory<Program> factory)
        {
            using (var scope = factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<DataContext>();

                if (!await db.Questions.AnyAsync())
                {
                    var question = new Question
                    {
                        Text = "What is the capital of France?",
                        Categories = new List<QuestionCategory>
                        {
                            new QuestionCategory { Name = "Geography" }
                        },
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Paris", User = "User1" }
                        }
                    };

                    db.Questions.Add(question);
                    await db.SaveChangesAsync();
                }
            }
        }

        [Fact]
        public async Task Test_GetQuestions_ReturnsAllQuestions()
        {
            var response = await _client.GetAsync("/question");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var questions = JsonSerializer.Deserialize<IEnumerable<object>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(questions);
            Assert.Equal(1, questions.Count());
        }

        [Fact]
        public async Task Test_GetQuestion_ExistingId_ReturnsQuestion()
        {
            var response = await _client.GetAsync("/question/1");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var question = JsonSerializer.Deserialize<Question>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(question);
            Assert.Equal("What is the capital of France?", question.Text);
        }

        [Fact]
        public async Task Test_GetQuestion_NonExistingId_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/question/999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Test_AddQuestion_ReturnsCreatedQuestion()
        {
            var newQuestion = new CreateQuestion
            {
                Text = "What is the largest ocean?",
                Categories = new List<CreateQuestionCategory>
                {
                    new CreateQuestionCategory { Name = "Geography" }
                },
                Answers = new List<CreateAnswer>
                {
                    new CreateAnswer { Text = "Pacific Ocean", User = "User2" }
                }
            };


            var response = await _client.PostAsJsonAsync("/question/add", newQuestion);
                response.EnsureSuccessStatusCode();

            var createdQuestionJson = await response.Content.ReadAsStringAsync();
            var createdQuestion = JsonSerializer.Deserialize<Question>(createdQuestionJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(createdQuestion);
            Assert.Equal("What is the largest ocean?", createdQuestion.Text);
        }

        [Fact]
        public async Task Test_UpdateQuestion_ExistingId_ReturnsNoContent()
        {
            var updatedQuestion = new CreateQuestion
            {
                Text = "What is the capital of Canada?",
                Categories = new List<CreateQuestionCategory>
                {
                    new CreateQuestionCategory { Name = "Geography" }
                },
                Answers = new List<CreateAnswer>
                {
                    new CreateAnswer { Text = "Ottawa", User = "User3" }
                }
            };

            var response = await _client.PutAsJsonAsync("/question/1", updatedQuestion);
            response.EnsureSuccessStatusCode();

            var getResponse = await _client.GetAsync("/question/1");
            var json = await getResponse.Content.ReadAsStringAsync();
            var question = JsonSerializer.Deserialize<Question>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Equal("What is the capital of Canada?", question.Text);
        }

        [Fact]
        public async Task Test_UpdateQuestion_NonExistingId_ReturnsNotFound()
        {
            var updatedQuestion = new CreateQuestion
            {
                Text = "Some random text",
                Categories = new List<CreateQuestionCategory>
                {
                    new CreateQuestionCategory
                    {
                        Name = "Valid Category Name"
                    }
                },
                Answers = new List<CreateAnswer>()
            };

            var response = await _client.PutAsJsonAsync("/question/999", updatedQuestion);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }


        [Fact]
        public async Task Test_DeleteQuestion_ExistingId_ReturnsNoContent()
        {
            var response = await _client.DeleteAsync("/question/1");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Test_DeleteQuestion_NonExistingId_ReturnsNotFound()
        {
            var response = await _client.DeleteAsync("/question/999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Test_GetCategories_ReturnsCategories()
        {
            var response = await _client.GetAsync("/question/categories");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var categories = JsonSerializer.Deserialize<IEnumerable<CreateQuestionCategory>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(categories);
            Assert.NotEmpty(categories);
        }
    }
}
