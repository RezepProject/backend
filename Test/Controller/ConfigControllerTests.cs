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
public class ConfigControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ConfigControllerTests(CustomWebApplicationFactory<Program> factory)
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

            if (!await db.Configs.AnyAsync())
            {
                db.Configs.Add(new Config { Title = "Test Config 1", Value = "Value1" });
                db.Configs.Add(new Config { Title = "Test Config 2", Value = "Value2" });
                await db.SaveChangesAsync();
            }
        }
    }

    [Fact]
    public async Task Test_GetConfigs_ReturnsAllConfigs()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/config");
        request.Headers.Add("Accept", "application/json");

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var configs = JsonSerializer.Deserialize<IEnumerable<Config>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(configs);
        Assert.NotEmpty(configs);
    }

    [Fact]
    public async Task Test_GetConfig_ExistingId_ReturnsCorrectConfig()
    {
        var response = await _client.GetAsync("/config/1");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var config = JsonSerializer.Deserialize<Config>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(config);
        Assert.Equal(1, config.Id);
        Assert.Equal("Test Config 1", config.Title);
    }

    [Fact]
    public async Task Test_GetConfig_NonExistingId_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/config/999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Test_AddConfig_ReturnsCreatedConfig()
    {
        var newConfig = new CreateConfig { Title = "New Config", Value = "NewValue" };
        var response = await _client.PostAsJsonAsync("/config", newConfig);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var createdConfig = JsonSerializer.Deserialize<Config>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(createdConfig);
        Assert.Equal("New Config", createdConfig.Title);
        Assert.Equal("NewValue", createdConfig.Value);
    }

    [Fact]
    public async Task Test_ChangeConfig_ExistingId_ReturnsNoContent()
    {
        var updatedConfig = new CreateConfig { Title = "Updated Config", Value = "UpdatedValue" };
        var response = await _client.PutAsJsonAsync("/config/1", updatedConfig);
        response.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync("/config/1");
        var json = await getResponse.Content.ReadAsStringAsync();
        var config = JsonSerializer.Deserialize<Config>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.Equal("Updated Config", config.Title);
        Assert.Equal("UpdatedValue", config.Value);
    }

    [Fact]
    public async Task Test_ChangeConfig_NonExistingId_ReturnsNotFound()
    {
        var updatedConfig = new CreateConfig { Title = "Updated Config", Value = "UpdatedValue" };
        var response = await _client.PutAsJsonAsync("/config/999", updatedConfig);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Test_DeleteConfig_ExistingId_ReturnsNoContent()
    {
        var response = await _client.DeleteAsync("/config/1");
        response.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync("/config/1");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task Test_DeleteConfig_NonExistingId_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync("/config/999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
