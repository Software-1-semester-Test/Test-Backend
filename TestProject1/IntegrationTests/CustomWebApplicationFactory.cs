using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Test_Backend.Services;
using Test_Backend.Models;
using MySql.Data.MySqlClient;

namespace TestProject1.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        
        // Find the Test-Backend project directory
        var currentDirectory = Directory.GetCurrentDirectory();
        string backendPath;
        
        // When running tests, we're in TestProject1/bin/Debug/net9.0
        // We need to go up to the solution directory and then into Test-Backend
        if (currentDirectory.Contains("TestProject1"))
        {
            // Go up to solution root
            var solutionDir = Directory.GetParent(currentDirectory);
            while (solutionDir != null && !Directory.Exists(Path.Combine(solutionDir.FullName, "Test-Backend")))
            {
                solutionDir = solutionDir.Parent;
            }
            
            if (solutionDir != null)
            {
                backendPath = Path.Combine(solutionDir.FullName, "Test-Backend");
            }
            else
            {
                throw new InvalidOperationException("Could not find Test-Backend project directory");
            }
        }
        else
        {
            backendPath = currentDirectory;
        }
        
        builder.UseContentRoot(backendPath);
        
        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Load configuration from Test-Backend project
            config.AddJsonFile("appsettings.json", optional: true)
                  .AddEnvironmentVariables();
                  
            // Override connection string for tests if needed
            config.AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:DefaultConnection"] = "Server=localhost;Database=testdb;User=root;Password=test;"
            }!);
        });

        builder.ConfigureServices(services =>
        {
            // Replace AddressService with mock version that doesn't use database
            services.RemoveAll<AddressService>();
            services.AddTransient<AddressService>(sp => new MockAddressService());
        });
    }
}

// Mock AddressService that doesn't require database connection
public class MockAddressService : AddressService
{
    private static readonly Random _rand = new Random();
    private static readonly string[] TestPostalCodes = { "1000", "2000", "3000", "4000", "5000", "8000" };
    private static readonly string[] TestTowns = { "København", "Aarhus", "Odense", "Aalborg", "Esbjerg", "Randers" };

    public MockAddressService() : base()
    {
    }

    // Override the virtual method to avoid database call
    public override Adress GetRandomAddress()
    {
        var address = new Adress
        {
            Street = RandomStreet(),
            Number = RandomNumber(),
            Floor = RandomFloor(),
            Door = RandomDoor(),
            PostalCode = TestPostalCodes[_rand.Next(TestPostalCodes.Length)],
            Town = TestTowns[_rand.Next(TestTowns.Length)]
        };

        return address;
    }
}
