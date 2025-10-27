using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Test_Backend.Interfaces;
using Test_Backend.Models;

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
        });

        builder.ConfigureServices(services =>
        {
            // Remove the real IAddressService and replace with mock
            services.RemoveAll<IAddressService>();
            services.AddTransient<IAddressService, MockAddressService>();
        });
    }
}

// Mock implementation of IAddressService that doesn't require database
public class MockAddressService : IAddressService
{
    private static readonly Random _rand = new Random();
    private static readonly string[] TestPostalCodes = { "1000", "2000", "3000", "4000", "5000", "8000" };
    private static readonly string[] TestTowns = { "København", "Aarhus", "Odense", "Aalborg", "Esbjerg", "Randers" };
    private static readonly string[] TestStreets = { "Hovedgade", "Strandvej", "Parkvej", "Skolegade", "Kirkevej" };

    public Adress GetRandomAddress()
    {
        return new Adress
        {
            Street = GenerateStreet(),
            Number = GenerateNumber(),
            Floor = GenerateFloor(),
            Door = GenerateDoor(),
            PostalCode = TestPostalCodes[_rand.Next(TestPostalCodes.Length)],
            Town = TestTowns[_rand.Next(TestTowns.Length)]
        };
    }

    private string GenerateStreet()
    {
        return TestStreets[_rand.Next(TestStreets.Length)];
    }

    private string GenerateNumber()
    {
        int number = _rand.Next(1, 200);
        string letter = _rand.NextDouble() < 0.3 ? ((char)_rand.Next('A', 'D')).ToString() : "";
        return $"{number}{letter}";
    }

    private string GenerateFloor()
    {
        return _rand.NextDouble() < 0.2 ? "st" : _rand.Next(1, 10).ToString();
    }

    private string GenerateDoor()
    {
        string[] fixedDoors = { "th", "mf", "tv" };
        if (_rand.NextDouble() < 0.3)
            return fixedDoors[_rand.Next(fixedDoors.Length)];

        string letter = ((char)_rand.Next('a', 'd')).ToString();
        return letter;
    }
}
