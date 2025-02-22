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
/*
[Collection("Sequential Tests")]
public class SettingsControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public SettingsControllerTests(CustomWebApplicationFactory<Program> factory)
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

            if (!await db.Settings.AnyAsync())
            {
                db.Settings.Add(new Setting
                {
                    Name = "Default Setting",
                    BackgroundImage = "default.png",
                    Language = "en",
                    TalkingSpeed = 1.5,
                    GreetingMessage = "Hello, how can I assist you?",
                    State = true,
                    AiInUse = "Yes"
                });
                await db.SaveChangesAsync();
            }
        }
    }

    [Fact]
    public async Task Test_GetSettings_ReturnsAllSettings()
    {
        var response = await _client.GetAsync("/settings");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var settings = JsonSerializer.Deserialize<IEnumerable<Setting>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(settings);
        Assert.NotEmpty(settings);
    }

    [Fact]
    public async Task Test_GetSetting_ExistingId_ReturnsCorrectSetting()
    {
        var response = await _client.GetAsync("/settings/2");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var setting = JsonSerializer.Deserialize<Setting>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(setting);
        Assert.Equal(1, setting.Id);
        Assert.Equal("Default Setting", setting.Name);
    }

    [Fact]
    public async Task Test_GetSetting_NonExistingId_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/settings/999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Test_CreateSetting_ReturnsCreatedSetting()
    {
        var newSetting = new CreateSetting
        {
            Name = "New Setting",
            BackgroundImage = "new_image.png",
            Language = "fr",
            TalkingSpeed = 1.2,
            GreetingMessage = "Bonjour, comment puis-je vous aider?",
            State = true,
            AiInUse = "No"
        };

        var response = await _client.PostAsJsonAsync("/settings", newSetting);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var createdSetting = JsonSerializer.Deserialize<Setting>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(createdSetting);
        Assert.Equal("New Setting", createdSetting.Name);
    }

    [Fact]
    public async Task Test_UpdateSetting_ExistingId_ReturnsNoContent()
    {
        var updatedSetting = new CreateSetting
        {
            Name = "Updated Setting",
            BackgroundImage = "updated_image.png",
            Language = "en",
            TalkingSpeed = 1.0,
            GreetingMessage = "Updated greeting message.",
            State = false,
            AiInUse = "Yes"
        };
        
        var response = await _client.PutAsJsonAsync("/settings/1", updatedSetting);
        response.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync("/settings/1");
        var json = await getResponse.Content.ReadAsStringAsync();
        var setting = JsonSerializer.Deserialize<Setting>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.Equal("Updated Setting", setting.Name);
        Assert.Equal("updated_image.png", setting.BackgroundImage);
        Assert.False(setting.State);
    }

    [Fact]
    public async Task Test_UpdateSetting_NonExistingId_ReturnsNotFound()
    {
        var updatedSetting = new CreateSetting
        {
            Name = "Updated Setting",
            BackgroundImage = "updated_image.png",
            Language = "en",
            TalkingSpeed = 1.0,
            GreetingMessage = "Updated greeting message.",
            State = false,
            AiInUse = "Yes"
        };

        var response = await _client.PutAsJsonAsync("/settings/999", updatedSetting);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
} */
