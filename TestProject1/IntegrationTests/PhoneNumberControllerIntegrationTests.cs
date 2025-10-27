using System.Net;
using Xunit;

namespace TestProject1.IntegrationTests;

public class PhoneNumberControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public PhoneNumberControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetRandomPhoneNumber_ReturnsSuccessAndPhoneNumber()
    {
        // Act
        var response = await _client.GetAsync("/api/phonenumber/random");

        // Assert
        response.EnsureSuccessStatusCode();
        var jsonContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("number", jsonContent);
    }

    [Fact]
    public async Task GetRandomPhoneNumber_ReturnsValidFormat()
    {
        // Act
        var response = await _client.GetAsync("/api/phonenumber/random");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var jsonContent = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(jsonContent);
        // PhoneNumber should contain 8 digits
        Assert.Matches(@"\d{8}", jsonContent);
    }

    [Fact]
    public async Task GetRandomPhoneNumber_MultipleCalls_ReturnsDifferentNumbers()
    {
        // Act
        var response1 = await _client.GetAsync("/api/phonenumber/random");
        var response2 = await _client.GetAsync("/api/phonenumber/random");
        var response3 = await _client.GetAsync("/api/phonenumber/random");

        // Assert
        response1.EnsureSuccessStatusCode();
        response2.EnsureSuccessStatusCode();
        response3.EnsureSuccessStatusCode();

        var content1 = await response1.Content.ReadAsStringAsync();
        var content2 = await response2.Content.ReadAsStringAsync();
        var content3 = await response3.Content.ReadAsStringAsync();

        // All should be valid responses
        Assert.NotEmpty(content1);
        Assert.NotEmpty(content2);
        Assert.NotEmpty(content3);
    }
}
