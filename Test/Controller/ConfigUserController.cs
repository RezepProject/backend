using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using backend;
using backend.Entities;
using backend.Util;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Test.Utils;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace Test.Controller;

[Collection("Sequential Tests")]
public class ConfigUserControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ConfigUserControllerTests(CustomWebApplicationFactory<Program> factory)
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

            if (!await db.ConfigUsers.AnyAsync())
            {
                db.ConfigUsers.Add(new ConfigUser 
                { 
                    FirstName = "Test", 
                    LastName = "User", 
                    Email = "test@example.com", 
                    Password = AuthenticationUtils.HashPassword("InitialPassword"), 
                    RoleId = 1 
                });

                await db.SaveChangesAsync();
            }
        }
    }

    [Fact]
    public async Task Test_GetUsers_ReturnsAllUsers()
    {
        var response = await _client.GetAsync("/configuser");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var users = JsonSerializer.Deserialize<IEnumerable<ReturnConfigUser>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(users);
        Assert.NotEmpty(users);
    }

    [Fact]
    public async Task Test_GetUser_NonExistingId_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/configuser/999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    

    [Fact]
    public async Task Test_DeleteUser_ExistingId_ReturnsNoContent()
    {
        var response = await _client.DeleteAsync("/configuser/1");
        response.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync("/configuser/1");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task Test_DeleteUser_NonExistingId_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync("/configuser/999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Test_PutUser_ExistingId_ReturnsNoContent()
    {
        var updatedUser = new ChangeConfigUser 
        { 
            FirstName = "Updated", 
            LastName = "User", 
            Email = "updated@example.com", 
            RoleId = 1 
        };
        var response = await _client.PutAsJsonAsync("/configuser/1", updatedUser);
        response.EnsureSuccessStatusCode();

        // Confirm the update persisted
        var getResponse = await _client.GetAsync("/configuser/1");
        var json = await getResponse.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<ReturnConfigUser>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.Equal("Updated", user.FirstName);
        Assert.Equal("User", user.LastName);
        Assert.Equal("updated@example.com", user.Email);
    }

    [Fact]
    public async Task Test_PutUser_NonExistingId_ReturnsNotFound()
    {
        var updatedUser = new ChangeConfigUser 
        { 
            FirstName = "Updated", 
            LastName = "User", 
            Email = "updated@example.com", 
            RoleId = 1 
        };
        var response = await _client.PutAsJsonAsync("/configuser/999", updatedUser);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    

    [Fact]
    public async Task Test_ResendInvitation_NonExistingId_ReturnsNotFound()
    {
        var response = await _client.PostAsync("/configuser/invitation/999/resend", null);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Test_DeleteInvitation_NonExistingId_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync("/configuser/invitation/999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
