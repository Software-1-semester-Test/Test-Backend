using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace TestProject1.IntegrationTests;

public class NameControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public NameControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetRandom_ReturnsSuccessAndName()
    {
        // Act
        var response = await _client.GetAsync("/api/name/random");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(content);
        Assert.Contains("name", content);
        Assert.Contains("surname", content);
        Assert.Contains("gender", content);
    }

    [Fact]
    public async Task GetRandom_WithDob_ReturnsNameAndDateOfBirth()
    {
        // Act
        var response = await _client.GetAsync("/api/name/random?includeDob=true");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var jsonContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("name", jsonContent);
        Assert.Contains("surname", jsonContent);
        Assert.Contains("gender", jsonContent);
        Assert.Contains("dateOfBirth", jsonContent);
    }

    [Fact]
    public async Task GetAll_ReturnsListOfNames()
    {
        // Act
        var response = await _client.GetAsync("/api/name/all");

        // Assert
        response.EnsureSuccessStatusCode();
        var jsonContent = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(jsonContent);
        Assert.Contains("[", jsonContent); // Should be a JSON array
    }

    [Theory]
    [InlineData("male")]
    [InlineData("female")]
    public async Task GetByGender_ReturnsFilteredNames(string gender)
    {
        // Act
        var response = await _client.GetAsync($"/api/name/gender/{gender}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var jsonContent = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(jsonContent);
    }

    [Fact]
    public async Task GetByGender_InvalidGender_StillReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/api/name/gender/invalid");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
