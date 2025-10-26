using Test_Backend.Services;
using Test_Backend.Models;

namespace Test_Backend.Tests;

/// <summary>
/// White-box tests for PhoneNumberService - covers branch coverage and loop execution paths
/// </summary>
public class PhoneNumberServiceTests
{
    private readonly PhoneNumberService _phoneNumberService;

    public PhoneNumberServiceTests()
    {
        _phoneNumberService = new PhoneNumberService();
    }

    // WHITE-BOX: Tests core path - object creation, length validation, digit format
    [Fact]
    public void GetRandomPhoneNumber_ReturnsValid8DigitNumber()
    {
        PhoneNumber result = _phoneNumberService.GetRandomPhoneNumber();

        Assert.NotNull(result);
        Assert.Equal(8, result.Number.Length);
        Assert.Matches("^[0-9]{8}$", result.Number);
    }

    // WHITE-BOX BRANCH: Tests loop with 1-digit prefix - remainingDigits = 8 - 1 = 7
    [Fact]
    public void GetRandomPhoneNumber_OneDigitPrefix_Generates7MoreDigits()
    {
        for (int i = 0; i < 500; i++)
        {
            PhoneNumber result = _phoneNumberService.GetRandomPhoneNumber();
            if (result.Number.StartsWith("2") && !result.Number.StartsWith("20"))
            {
                Assert.Matches("^2[0-9]{7}$", result.Number);
                return;
            }
        }
        Assert.Fail("No 1-digit prefix found");
    }

    // WHITE-BOX BRANCH: Tests loop with 2-digit prefix - remainingDigits = 8 - 2 = 6
    [Fact]
    public void GetRandomPhoneNumber_TwoDigitPrefix_Generates6MoreDigits()
    {
        string[] prefixes = ["30", "40", "50", "60", "71"];
        for (int i = 0; i < 500; i++)
        {
            PhoneNumber result = _phoneNumberService.GetRandomPhoneNumber();
            foreach (var prefix in prefixes)
            {
                if (result.Number.StartsWith(prefix))
                {
                    Assert.Equal(6, result.Number.Substring(2).Length);
                    return;
                }
            }
        }
        Assert.Fail("No 2-digit prefix found");
    }

    // WHITE-BOX BRANCH: Tests loop with 3-digit prefix - remainingDigits = 8 - 3 = 5
    [Fact]
    public void GetRandomPhoneNumber_ThreeDigitPrefix_Generates5MoreDigits()
    {
        string[] prefixes = ["342", "356", "431", "662"];
        for (int i = 0; i < 500; i++)
        {
            PhoneNumber result = _phoneNumberService.GetRandomPhoneNumber();
            foreach (var prefix in prefixes)
            {
                if (result.Number.StartsWith(prefix))
                {
                    Assert.Equal(5, result.Number.Substring(3).Length);
                    return;
                }
            }
        }
        Assert.Fail("No 3-digit prefix found");
    }

    // WHITE-BOX: Tests Rand.Next(0, 10) produces valid digits in loop
    [Fact]
    public void GetRandomPhoneNumber_AllDigitsInRange0To9()
    {
        for (int i = 0; i < 50; i++)
        {
            PhoneNumber result = _phoneNumberService.GetRandomPhoneNumber();
            foreach (char c in result.Number)
            {
                int digit = int.Parse(c.ToString());
                Assert.InRange(digit, 0, 9);
            }
        }
    }

    // WHITE-BOX: Tests randomness - different prefixes and digits generated
    [Fact]
    public void GetRandomPhoneNumber_ProducesVariedResults()
    {
        HashSet<string> numbers = new HashSet<string>();
        for (int i = 0; i < 100; i++)
        {
            numbers.Add(_phoneNumberService.GetRandomPhoneNumber().Number);
        }
        Assert.True(numbers.Count > 50, $"Only {numbers.Count}/100 unique numbers");
    }
}

