using Test_Backend.Services;
using Test_Backend.Models;

namespace Test_Backend.Tests;

public class CprServiceTests
{
    private readonly CprService _cprService;

    public CprServiceTests()
    {
        _cprService = new CprService();
    }

    #region Last digit tests
    [Fact]
    public void GenerateCpr_FemalePerson_ShouldHaveEvenLastDigit()
    {
        DateTime birthDate = new DateTime(1990, 5, 15);
        string gender = "female";

        Cpr result = _cprService.GenerateCpr(birthDate, gender);

        Assert.NotNull(result);
        Assert.NotNull(result.Number);
        Assert.Equal(10, result.Number.Length);

        string expectedDatePart = "150590";
        Assert.StartsWith(expectedDatePart, result.Number);

        char lastDigit = result.Number[9];
        int lastDigitValue = int.Parse(lastDigit.ToString());
        Assert.True(lastDigitValue % 2 == 0, $"Last digit {lastDigitValue} should be even for female");
    }

    [Fact]
    public void GenerateCpr_MalePerson_ShouldHaveOddLastDigit()
    {
        DateTime birthDate = new DateTime(1985, 12, 25);
        string gender = "male";

        Cpr result = _cprService.GenerateCpr(birthDate, gender);

        Assert.NotNull(result);
        Assert.NotNull(result.Number);
        Assert.Equal(10, result.Number.Length);

        string expectedDatePart = "251285";
        Assert.StartsWith(expectedDatePart, result.Number);

        char lastDigit = result.Number[9];
        int lastDigitValue = int.Parse(lastDigit.ToString());
        Assert.True(lastDigitValue % 2 == 1, $"Last digit {lastDigitValue} should be odd for male");
    }
    #endregion

    #region Validate cpr tests
    [Fact]
    public void ValidateCprWithDateOfBirth_WithNullCpr_ShouldReturnFalse()
    {
        string? cprNumber = null;
        DateTime dateOfBirth = new DateTime(1999, 10, 3);

        bool result = _cprService.ValidateCprWithDateOfBirth(cprNumber!, dateOfBirth);

        Assert.False(result);
    }

    [Fact]
    public void ValidateCprWithDateOfBirth_WithEmptyCpr_ShouldReturnFalse()
    {
        string cprNumber = "";
        DateTime dateOfBirth = new DateTime(2023, 2, 7);

        bool result = _cprService.ValidateCprWithDateOfBirth(cprNumber, dateOfBirth);

        Assert.False(result);
    }

    [Fact]
    public void ValidateCprWithDateOfBirth_WithWrongLength_ShouldReturnFalse()
    {
        string cprNumber = "123456789";
        DateTime dateOfBirth = new DateTime(2017, 6, 3);

        bool result = _cprService.ValidateCprWithDateOfBirth(cprNumber, dateOfBirth);

        Assert.False(result);
    }

    [Fact]
    public void ValidateCprWithDateOfBirth_WithMismatchedDate_ShouldReturnFalse()
    {
        string cprNumber = "1505941234";
        DateTime dateOfBirth = new DateTime(1990, 5, 15);

        bool result = _cprService.ValidateCprWithDateOfBirth(cprNumber, dateOfBirth);

        Assert.False(result);
    }

    [Fact]
    public void ValidateCprWithDateOfBirth_WithValidCpr_ShouldReturnTrue()
    {
        string cprNumber = "1505901234";
        DateTime birthDate = new DateTime(1990, 5, 15);

        bool result = _cprService.ValidateCprWithDateOfBirth(cprNumber, birthDate);

        Assert.True(result);
    }
    #endregion

    [Fact]
    public void GenerateCpr_WithUppercaseFemale_ShouldHaveEvenLastDigit()
    {
        DateTime birthDate = new DateTime(2000, 1, 1);
        string gender = "FEMALE";

        Cpr result = _cprService.GenerateCpr(birthDate, gender);

        char lastDigit = result.Number[9];
        int lastDigitValue = int.Parse(lastDigit.ToString());
        Assert.True(lastDigitValue % 2 == 0, "Uppercase FEMALE should still produce even last digit");
    }
}