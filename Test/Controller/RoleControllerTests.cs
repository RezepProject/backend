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
public class RoleControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public RoleControllerTests(CustomWebApplicationFactory<Program> factory)
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

            if (!await db.Roles.AnyAsync())
            {
                db.Roles.Add(new Role { Name = "Admin" });
                db.Roles.Add(new Role { Name = "User" });
                await db.SaveChangesAsync();
            }
        }
    }

    [Fact]
    public async Task Test_GetRoles_ReturnsAllRoles()
    {
        var response = await _client.GetAsync("/role");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var roles = JsonSerializer.Deserialize<IEnumerable<Role>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(roles);
        Assert.NotEmpty(roles);
    }

    [Fact]
    public async Task Test_GetRole_ExistingId_ReturnsCorrectRole()
    {
        var response = await _client.GetAsync("/role/1");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var role = JsonSerializer.Deserialize<Role>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(role);
        Assert.Equal(1, role.Id);
        Assert.Equal("ADMIN", role.Name);
    }

    [Fact]
    public async Task Test_GetRole_NonExistingId_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/role/999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Test_CreateRole_ReturnsCreatedRole()
    {
        var newRole = new CreateRole { Name = "Manager" };
        var response = await _client.PostAsJsonAsync("/role", newRole);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var createdRole = JsonSerializer.Deserialize<Role>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(createdRole);
        Assert.Equal("Manager", createdRole.Name);
    }

    [Fact]
    public async Task Test_UpdateRole_ExistingId_ReturnsNoContent()
    {
        var updatedRole = new CreateRole { Name = "Super Admin" };
        var response = await _client.PutAsJsonAsync("/role/1", updatedRole);
        response.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync("/role/1");
        var json = await getResponse.Content.ReadAsStringAsync();
        var role = JsonSerializer.Deserialize<Role>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.Equal("Super Admin", role.Name);
    }

    [Fact]
    public async Task Test_UpdateRole_NonExistingId_ReturnsNotFound()
    {
        var updatedRole = new CreateRole { Name = "Super Admin" };
        var response = await _client.PutAsJsonAsync("/role/999", updatedRole);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Test_DeleteRole_ExistingId_ReturnsNoContent()
    {
        var response = await _client.DeleteAsync("/role/1");
        response.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync("/role/1");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task Test_DeleteRole_NonExistingId_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync("/role/999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
