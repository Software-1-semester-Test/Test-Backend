using Test_Backend.Services;
using Test_Backend.Models;
using Test_Backend.Interfaces;
using NSubstitute;
using Microsoft.AspNetCore.Hosting;

namespace Test_Backend.Tests;

public class NameServiceTests : INameService
{
    private readonly NameService _nameService;
    private readonly IWebHostEnvironment _mockEnvironment;

    public NameServiceTests()
    {
        _mockEnvironment = Substitute.For<IWebHostEnvironment>();
        _mockEnvironment.ContentRootPath.Returns(Directory.GetCurrentDirectory());

        _nameService = new NameService(_mockEnvironment);
    }

    [Fact]
    public void GetRandomName_WithNamesAvailable_ShouldReturnName()
    {
        Name result = _nameService.GetRandomName();

        Assert.NotNull(result);
        Assert.NotNull(result.FirstName);
        Assert.NotNull(result.LastName);
        Assert.NotNull(result.Gender);
    }

    [Fact]
    public void GetNames_ShouldReturnAllNames()
    {
        var result = _nameService.GetNames();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void GetByGender_WithFemale_ShouldReturnOnlyFemales()
    {
        string gender = "female";

        var result = _nameService.GetByGender(gender);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.All(result, name => Assert.Equal("female", name.Gender));
    }

    [Fact]
    public void GetByGender_WithMale_ShouldReturnOnlyMales()
    {
        string gender = "male";

        var result = _nameService.GetByGender(gender);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.All(result, name => Assert.Equal("male", name.Gender));
    }

    [Fact]
    public void GetByGender_WithNonExistentGender_ShouldReturnEmpty()
    {
        string gender = "nonexistent";

        var result = _nameService.GetByGender(gender);

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}