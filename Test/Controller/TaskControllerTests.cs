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
public class TaskControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TaskControllerTests(CustomWebApplicationFactory<Program> factory)
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

            // Seed some initial tasks
            if (!await db.Tasks.AnyAsync())
            {
                db.Tasks.Add(new EntityTask { Text = "Sample Task 1", Done = false });
                db.Tasks.Add(new EntityTask { Text = "Sample Task 2", Done = true });
                await db.SaveChangesAsync();
            }
        }
    }

    [Fact]
    public async Task Test_GetTasks_ReturnsAllTasks()
    {
        var response = await _client.GetAsync("/task");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var tasks = JsonSerializer.Deserialize<IEnumerable<EntityTask>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(tasks);
        Assert.NotEmpty(tasks);
    }

    [Fact]
    public async Task Test_GetTask_ExistingId_ReturnsCorrectTask()
    {
        var response = await _client.GetAsync("/task/1");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var task = JsonSerializer.Deserialize<EntityTask>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(task);
        Assert.Equal(1, task.Id);
        Assert.Equal("Sample Task 1", task.Text);
    }

    [Fact]
    public async Task Test_GetTask_NonExistingId_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/task/999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Test_CreateTask_ReturnsCreatedTask()
    {
        var newTask = new CreateTask { Text = "Newly Created Task", Done = false };
        
        var response = await _client.PostAsJsonAsync("/task", newTask);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var createdTask = JsonSerializer.Deserialize<EntityTask>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(createdTask);
        Assert.Equal("Newly Created Task", createdTask.Text);
        Assert.False(createdTask.Done); // Ensure the 'Done' property is set correctly
    }

    [Fact]
    public async Task Test_UpdateTask_ExistingId_ReturnsNoContent()
    {
        var updatedTask = new UpdateTask { Text = "Updated Task", Done = true };
        
        var response = await _client.PutAsJsonAsync("/task/1", updatedTask);
        response.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync("/task/1");
        var json = await getResponse.Content.ReadAsStringAsync();
        var task = JsonSerializer.Deserialize<EntityTask>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.Equal("Updated Task", task.Text);
        Assert.True(task.Done);
    }

    [Fact]
    public async Task Test_UpdateTask_NonExistingId_ReturnsNotFound()
    {
        var updatedTask = new UpdateTask { Text = "This Task Will Not Be Found", Done = false };

        var response = await _client.PutAsJsonAsync("/task/999", updatedTask);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Test_DeleteTask_ExistingId_ReturnsNoContent()
    {
        var response = await _client.DeleteAsync("/task/1");
        response.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync("/task/1");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task Test_DeleteTask_NonExistingId_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync("/task/999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
