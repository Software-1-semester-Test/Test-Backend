using Test_Backend.Services;
using Test_Backend.Models;

namespace Test_Backend.Tests;

public class PhoneNumberServiceTests
{
    private readonly PhoneNumberService _phoneService;
    public PhoneNumberServiceTests()
    {
        _phoneService = new PhoneNumberService();
    }

    [Fact]
    public void GeneratePhoneNumber_ReturnsValidPhoneNumber()
    {
        var phoneNumber = _phoneService.GetRandomPhoneNumber();

        Assert.NotNull(phoneNumber);
        Assert.NotNull(phoneNumber.Number);
        Assert.Equal(8, phoneNumber.Number.Length);
    }

    [Fact]
    public void GetRandomPhoneNumber_AlwaysGenerates8DigitsRegardlessOfPrefix()
    {
        for (int i = 0; i < 100; i++)
        {
            var phoneNumber = _phoneService.GetRandomPhoneNumber();

            Assert.Equal(8, phoneNumber.Number.Length);
            Assert.All(phoneNumber.Number, c => Assert.True(char.IsDigit(c)));
        }
    }
}