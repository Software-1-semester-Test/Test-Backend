using System.Net;
using Xunit;

namespace TestProject1.IntegrationTests;

public class CprControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public CprControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetRandomCpr_ReturnsSuccessAndCpr()
    {
        // Act
        var response = await _client.GetAsync("/api/cpr/random");

        // Assert
        response.EnsureSuccessStatusCode();
        var jsonContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("cprnummer", jsonContent);
    }

    [Fact]
    public async Task GetRandomCpr_CprNumberIsValid()
    {
        // Act
        var response = await _client.GetAsync("/api/cpr/random");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var jsonContent = await response.Content.ReadAsStringAsync();
        // CPR should be 10 digits
        Assert.Matches(@"\d{10}", jsonContent);
    }

    [Fact]
    public async Task GetCprWithNameGender_ReturnsCompleteCprData()
    {
        // Act
        var response = await _client.GetAsync("/api/cpr/random/with-name-gender");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var jsonContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("cprnummer", jsonContent);
        Assert.Contains("name", jsonContent);
        Assert.Contains("gender", jsonContent);
    }

    [Fact]
    public async Task GetCprWithNameGenderDob_ReturnsFullData()
    {
        // Act
        var response = await _client.GetAsync("/api/cpr/random/with-name-gender-dob");

        // Assert
        response.EnsureSuccessStatusCode();
        var jsonContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("cpr", jsonContent);
        Assert.Contains("dateOfBirth", jsonContent);
        Assert.Contains("name", jsonContent);
    }

    [Theory]
    [InlineData("2000-01-01", "male")]
    [InlineData("1990-05-15", "female")]
    [InlineData("1985-12-25", "Male")]
    [InlineData("2010-03-10", "Female")]
    public async Task GenerateCpr_WithValidInput_ReturnsGeneratedCpr(string date, string gender)
    {
        // Act
        var response = await _client.PostAsync(
            $"/api/cpr/generate?dateOfBirth={date}&gender={gender}", 
            null);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var jsonContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("number", jsonContent);
        Assert.Contains("dateOfBirth", jsonContent);
    }

    [Fact]
    public async Task GenerateCpr_WithoutGender_ReturnsBadRequest()
    {
        // Act
        var response = await _client.PostAsync(
            "/api/cpr/generate?dateOfBirth=2000-01-01", 
            null);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GenerateCpr_WithInvalidGender_ReturnsBadRequest()
    {
        // Act
        var response = await _client.PostAsync(
            "/api/cpr/generate?dateOfBirth=2000-01-01&gender=invalid", 
            null);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
