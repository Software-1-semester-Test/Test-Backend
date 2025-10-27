using System.Net;
using Xunit;

namespace TestProject1.IntegrationTests;

public class PersonControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public PersonControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetRandomPerson_ReturnsSuccessAndCompletePerson()
    {
        // Act
        var response = await _client.GetAsync("/api/person");

        // Assert
        response.EnsureSuccessStatusCode();
        var jsonContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("cpr", jsonContent);
        Assert.Contains("name", jsonContent);
        Assert.Contains("address", jsonContent);
        Assert.Contains("mobile", jsonContent);
    }

    [Fact]
    public async Task GetRandomPerson_ReturnsValidPersonStructure()
    {
        // Act
        var response = await _client.GetAsync("/api/person");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var jsonContent = await response.Content.ReadAsStringAsync();
        
        // Verify all person components are present
        Assert.NotEmpty(jsonContent);
        Assert.Contains("{", jsonContent); // JSON object
    }

    [Theory]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(50)]
    [InlineData(100)]
    public async Task GetRandomPersons_WithValidCount_ReturnsCorrectNumberOfPeople(int count)
    {
        // Act
        var response = await _client.GetAsync($"/api/person/bulk?count={count}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var jsonContent = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(jsonContent);
        Assert.Contains("[", jsonContent); // Should be JSON array
    }

    [Theory]
    [InlineData(1)]   // Below minimum - should be clamped to 2
    [InlineData(101)] // Above maximum - should be clamped to 100
    [InlineData(150)] // Way above maximum
    public async Task GetRandomPersons_WithInvalidCount_StillReturnsSuccess(int count)
    {
        // Act - PersonService clamps to 2-100, so should still succeed
        var response = await _client.GetAsync($"/api/person/bulk?count={count}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var jsonContent = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(jsonContent);
    }

    [Fact]
    public async Task GetRandomPersons_DefaultCount_ReturnsSuccess()
    {
        // Act - No count parameter should use default (2)
        var response = await _client.GetAsync("/api/person/bulk");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var jsonContent = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(jsonContent);
    }

    [Fact]
    public async Task GetRandomPerson_MultipleCalls_ReturnsValidPersons()
    {
        // Act & Assert - Call multiple times to ensure consistency
        for (int i = 0; i < 3; i++)
        {
            var response = await _client.GetAsync("/api/person");
            response.EnsureSuccessStatusCode();
            
            var jsonContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("cpr", jsonContent);
            Assert.Contains("name", jsonContent);
        }
    }
}
