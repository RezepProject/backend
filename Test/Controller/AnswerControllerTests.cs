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

namespace Test.Controller;

[Collection("Sequential Tests")]
public class AnswerControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AnswerControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        // Create a new HttpClient from the factory
        _client = factory.CreateClient();
        SeedDataAsync(factory).GetAwaiter().GetResult(); // Seed data once when the class is initialized
    }

    // This method seeds the database with test data
    private async Task SeedDataAsync(CustomWebApplicationFactory<Program> factory)
    {
        using (var scope = factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DataContext>();
            db.Database.EnsureCreated(); // Ensure the database is created

            if (!await db.Questions.AnyAsync())
            {
                var question = new Question
                {
                    Text = "What is the capital of France?",
                    Answers = new List<Answer>
                    {
                        new Answer { Text = "Paris", User = "User1" },
                        new Answer { Text = "Lyon", User = "User2" },
                        new Answer { Text = "Marseille", User = "User3" }
                    }
                };

                db.Questions.Add(question);
                await db.SaveChangesAsync(); // Persist the changes
            }
        }
    }

    [Fact]
    public async Task Test_GetAnswers_ReturnsAllAnswers()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/answer");
        request.Headers.Add("Accept", "application/json");

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var answers = JsonSerializer.Deserialize<IEnumerable<Answer>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(answers);
        Assert.Equal(9, answers.Count()); // Expected count based on seeded data
    }

    [Fact]
    public async Task Test_GetAnswer_ExistingId_ReturnsCorrectId()
    {
        var response = await _client.GetAsync("/answer/1"); // Assume the answer has ID 1
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var answer = JsonSerializer.Deserialize<Answer>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(answer);
        Assert.Equal(1, answer.Id); // Check if the returned answer has the expected ID
    }


    [Fact]
    public async Task Test_GetAnswer_NonExistingId_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/answer/999"); // Assume 999 does not exist
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode); // Expect 404 Not Found
    }

    [Fact]
    public async Task Test_AddAnswer_ReturnsCreatedAnswer()
    {
        var newAnswer = new Answer { Text = "New Answer", User = "User4" };
        var response = await _client.PostAsJsonAsync("/answer", newAnswer);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var createdAnswer = JsonSerializer.Deserialize<Answer>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(createdAnswer);
        Assert.Equal("New Answer", createdAnswer.Text); // Validate the text
    }

    [Fact]
    public async Task Test_UpdateAnswer_ExistingId_ReturnsNoContent()
    {
        var updatedAnswer = new Answer { Text = "Updated Answer", User = "User1" };
        var response = await _client.PutAsJsonAsync("/answer/1", updatedAnswer); // Assume there's an answer with ID 1
        response.EnsureSuccessStatusCode();

        // Further validation can be done by retrieving the updated answer
        var getResponse = await _client.GetAsync("/answer/1");
        var json = await getResponse.Content.ReadAsStringAsync();
        var answer = JsonSerializer.Deserialize<Answer>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.Equal("Updated Answer", answer.Text);
    }

    [Fact]
    public async Task Test_UpdateAnswer_NonExistingId_ReturnsNotFound()
    {
        var updatedAnswer = new Answer { Text = "Updated Answer", User = "User1" };
        var response = await _client.PutAsJsonAsync("/answer/999", updatedAnswer); // Assume 999 does not exist
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Test_DeleteAnswer_ExistingId_ReturnsNoContent()
    {
        var response = await _client.DeleteAsync("/answer/2"); // Assume there's an answer with ID 1
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Test_DeleteAnswer_NonExistingId_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync("/answer/999"); // Assume 999 does not exist
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
