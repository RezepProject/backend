using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using backend;
using backend.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Test.Utils;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace Test.Controller;
/*
[Collection("Sequential Tests")]
public class StreamingControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public StreamingControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        factory.InitializeDatabase();
    }

    [Fact]
    public async Task Test_SendFrame_WithValidBase64Image_ReturnsOk()
    {
        // Arrange: Create a valid Base64 encoded image
        using var image = new Bitmap(100, 100); // Create a dummy 100x100 image
        using var ms = new MemoryStream();
        image.Save(ms, ImageFormat.Png);
        var base64Image = Convert.ToBase64String(ms.ToArray());

        var frameObject = new StreamingController.FrameObject { Data = base64Image };

        // Act: Send the frame to the controller
        var response = await _client.PostAsJsonAsync("/streaming", frameObject);

        // Assert: Check the response status code and content
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var json = await response.Content.ReadAsStringAsync();
        var faces = JsonSerializer.Deserialize<Rectangle[]>(json);

        // Validate that face detection was attempted (we expect an array but it may be empty)
        Assert.NotNull(faces);
    }

    [Fact]
    public async Task Test_SendFrame_WithInvalidBase64Image_ReturnsBadRequest()
    {
        // Arrange: Create an invalid Base64 string
        var frameObject = new StreamingController.FrameObject { Data = "not-a-base64-string" };

        // Act: Send the invalid frame to the controller
        var response = await _client.PostAsJsonAsync("/streaming", frameObject);

        // Assert: Check that the response is BadRequest
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Test_SendFrame_WithEmptyFrame_ReturnsBadRequest()
    {
        // Arrange: Create an empty frame object
        var frameObject = new StreamingController.FrameObject { Data = "" };

        // Act: Send the empty frame to the controller
        var response = await _client.PostAsJsonAsync("/streaming", frameObject);

        // Assert: The response should be BadRequest
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
*/