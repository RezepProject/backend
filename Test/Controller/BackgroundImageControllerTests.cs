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
public class BackgroundImageControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public BackgroundImageControllerTests(CustomWebApplicationFactory<Program> factory)
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

            if (!await db.BackgroundImages.AnyAsync())
            {
                var backgroundImage = new BackgroundImage
                {
                    Base64Image = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUA..."
                };

                db.BackgroundImages.Add(backgroundImage);
                await db.SaveChangesAsync();
            }
        }
    }

    [Fact]
    public async Task Test_GetBackgroundImages_ReturnsAllImages()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/backgroundimage");
        request.Headers.Add("Accept", "application/json");

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var images = JsonSerializer.Deserialize<IEnumerable<BackgroundImage>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(images);
        Assert.NotEmpty(images);
    }

    [Fact]
    public async Task Test_GetBackgroundImage_ExistingId_ReturnsCorrectImage()
    {
        var response = await _client.GetAsync("/backgroundimage/1");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var image = JsonSerializer.Deserialize<BackgroundImage>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(image);
        Assert.Equal(1, image.Id);
    }

    [Fact]
    public async Task Test_GetBackgroundImage_NonExistingId_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/backgroundimage/999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Test_AddBackgroundImage_ReturnsCreatedImage()
    {
        var newImage = new CreateBackgroundImage
        {
            Base64Image = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAIAAAD8nEClAAAAmklEQVRIDbWVzU4bZx3XHf/aQYZp5iSpQs6RZKYFh7FVh9WyHAgRHZGlGGFl+9OwyG19NqFMd56FLw8j38if3pxsX8zvYF1kekO4eZh9rHZP7ybbR5XyJ5OYtAsfT9SbxPfb+/B0D9cI6flzZoyVr5F5jQ26pbmrLeC6M9sLk5jmrT/LMghgg5y3w9K2jtRKlCy2RRv53l92Dq3wQakEoVG1Vrqxwh9Ejkt8+qPNe+gZUwXMrt/WjeXoxbe+NqDF7X2HqzDZmP9kttEHFN5NzzcTmA=="

        };

        var response = await _client.PostAsJsonAsync("/backgroundimage", newImage);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var createdImage = JsonSerializer.Deserialize<BackgroundImage>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(createdImage);
        Assert.Equal(newImage.Base64Image, createdImage.Base64Image);
    }


    [Fact]
    public async Task Test_DeleteBackgroundImage_ExistingId_ReturnsNoContent()
    {
        var response = await _client.DeleteAsync("/backgroundimage/1");
        response.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync("/backgroundimage/1");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task Test_DeleteBackgroundImage_NonExistingId_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync("/backgroundimage/999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
