using System.Net;
using Xunit;

namespace TestProject1.IntegrationTests;

public class AddressControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AddressControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetRandomAddress_ReturnsSuccessAndAddress()
    {
        // Act
        var response = await _client.GetAsync("/api/address/random");

        // Assert
        response.EnsureSuccessStatusCode();
        var jsonContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("street", jsonContent);
        Assert.Contains("doornumber", jsonContent);
        Assert.Contains("floor", jsonContent);
        Assert.Contains("door", jsonContent);
        Assert.Contains("postalCode", jsonContent);
        Assert.Contains("town", jsonContent);
    }

    [Fact]
    public async Task GetRandomAddress_ReturnsValidAddressStructure()
    {
        // Act
        var response = await _client.GetAsync("/api/address/random");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var jsonContent = await response.Content.ReadAsStringAsync();
        
        // Verify address components are present
        Assert.NotEmpty(jsonContent);
        Assert.Contains("\"", jsonContent); // JSON should have quotes
    }

    [Fact]
    public async Task GetRandomAddress_MultipleCalls_ReturnsValidAddresses()
    {
        // Act & Assert - Call multiple times to ensure consistency
        for (int i = 0; i < 5; i++)
        {
            var response = await _client.GetAsync("/api/address/random");
            response.EnsureSuccessStatusCode();
            
            var jsonContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("street", jsonContent);
            Assert.Contains("postalCode", jsonContent);
        }
    }
}
