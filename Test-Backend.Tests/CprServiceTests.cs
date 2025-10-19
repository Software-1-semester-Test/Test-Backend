using Test_Backend.Services;
using Test_Backend.Models;

namespace Test_Backend.Tests;

/// <summary>
/// White-box tests for CprService
/// These tests examine the internal logic and code paths of the CPR generation methods
/// </summary>
public class CprServiceTests
{
    // The service we're testing - created fresh for each test
    private readonly CprService _cprService;

    public CprServiceTests()
    {
        // Setup: Create a new instance of CprService before each test
        _cprService = new CprService();
    }

    #region GenerateCpr Tests - Testing specific date and gender combinations

    /// <summary>
    /// WHITE-BOX TEST: Tests the "female" branch in GenerateLastDigitByGender
    /// 
    /// What we're testing:
    /// - When gender is "female", the last digit should be EVEN (0, 2, 4, 6, 8)
    /// - The first 6 digits should match the date format "ddMMyy"
    /// - Total length should be exactly 10 digits
    /// </summary>
    [Fact]
    public void GenerateCpr_WithFemalePerson_ShouldHaveEvenLastDigit()
    {
        // Arrange: Set up test data
        DateTime birthDate = new DateTime(1990, 5, 15); // May 15, 1990
        string gender = "female";

        // Act: Call the method we're testing
        Cpr result = _cprService.GenerateCpr(birthDate, gender);

        // Assert: Verify the results
        Assert.NotNull(result);
        Assert.NotNull(result.Number);
        Assert.Equal(10, result.Number.Length); // CPR must be 10 digits
        
        // Check the first 6 digits match the date (ddMMyy format)
        string expectedDatePart = "150590"; // 15th May 1990
        Assert.StartsWith(expectedDatePart, result.Number);
        
        // WHITE-BOX: Check the last digit is even (testing the female branch)
        char lastDigit = result.Number[9];
        int lastDigitValue = int.Parse(lastDigit.ToString());
        Assert.True(lastDigitValue % 2 == 0, $"Last digit {lastDigitValue} should be even for female");
        
        // Verify the date of birth is stored correctly
        Assert.Equal(birthDate, result.DateOfBirth);
    }

    /// <summary>
    /// WHITE-BOX TEST: Tests the "male" branch in GenerateLastDigitByGender
    /// 
    /// What we're testing:
    /// - When gender is "male", the last digit should be ODD (1, 3, 5, 7, 9)
    /// - The first 6 digits should match the date format "ddMMyy"
    /// - Total length should be exactly 10 digits
    /// </summary>
    [Fact]
    public void GenerateCpr_WithMalePerson_ShouldHaveOddLastDigit()
    {
        // Arrange: Set up test data
        DateTime birthDate = new DateTime(1985, 12, 25); // December 25, 1985
        string gender = "male";

        // Act: Call the method we're testing
        Cpr result = _cprService.GenerateCpr(birthDate, gender);

        // Assert: Verify the results
        Assert.NotNull(result);
        Assert.NotNull(result.Number);
        Assert.Equal(10, result.Number.Length);
        
        // Check the first 6 digits match the date (ddMMyy format)
        string expectedDatePart = "251285"; // 25th December 1985
        Assert.StartsWith(expectedDatePart, result.Number);
        
        // WHITE-BOX: Check the last digit is odd (testing the male branch)
        char lastDigit = result.Number[9];
        int lastDigitValue = int.Parse(lastDigit.ToString());
        Assert.True(lastDigitValue % 2 == 1, $"Last digit {lastDigitValue} should be odd for male");
        
        // Verify the date of birth is stored correctly
        Assert.Equal(birthDate, result.DateOfBirth);
    }

    /// <summary>
    /// WHITE-BOX TEST: Tests edge case with case-insensitive gender comparison
    /// 
    /// What we're testing:
    /// - The code uses StringComparison.OrdinalIgnoreCase
    /// - "FEMALE" (uppercase) should work the same as "female"
    /// </summary>
    [Fact]
    public void GenerateCpr_WithUppercaseFemale_ShouldHaveEvenLastDigit()
    {
        // Arrange
        DateTime birthDate = new DateTime(2000, 1, 1);
        string gender = "FEMALE"; // Testing uppercase

        // Act
        Cpr result = _cprService.GenerateCpr(birthDate, gender);

        // Assert: Should still generate even digit for female
        char lastDigit = result.Number[9];
        int lastDigitValue = int.Parse(lastDigit.ToString());
        Assert.True(lastDigitValue % 2 == 0, "Uppercase FEMALE should still produce even last digit");
    }

    /// <summary>
    /// WHITE-BOX TEST: Tests date formatting with single-digit day/month
    /// 
    /// What we're testing:
    /// - Date formatting with leading zeros (e.g., "01" not "1")
    /// - Tests boundary date values
    /// </summary>
    [Fact]
    public void GenerateCpr_WithSingleDigitDayAndMonth_ShouldFormatCorrectly()
    {
        // Arrange: Date with single-digit day and month
        DateTime birthDate = new DateTime(1995, 3, 7); // March 7, 1995
        string gender = "male";

        // Act
        Cpr result = _cprService.GenerateCpr(birthDate, gender);

        // Assert: Should have leading zeros
        string expectedDatePart = "070395"; // 07th March 1995
        Assert.StartsWith(expectedDatePart, result.Number);
    }

    #endregion

    #region ValidateCprWithDateOfBirth Tests - Testing validation logic paths

    /// <summary>
    /// WHITE-BOX TEST: Tests the "valid CPR" path
    /// 
    /// What we're testing:
    /// - When CPR is 10 digits and date matches, should return true
    /// </summary>
    [Fact]
    public void ValidateCprWithDateOfBirth_WithValidCpr_ShouldReturnTrue()
    {
        // Arrange
        DateTime birthDate = new DateTime(1990, 5, 15);
        string validCpr = "1505901234"; // Correct format: 15th May 1990 + random digits

        // Act
        bool result = _cprService.ValidateCprWithDateOfBirth(validCpr, birthDate);

        // Assert
        Assert.True(result, "Valid CPR matching birth date should return true");
    }

    /// <summary>
    /// WHITE-BOX TEST: Tests the "null CPR" path
    /// 
    /// What we're testing:
    /// - The first condition: string.IsNullOrEmpty(cprNumber)
    /// - Should return false for null input
    /// </summary>
    [Fact]
    public void ValidateCprWithDateOfBirth_WithNullCpr_ShouldReturnFalse()
    {
        // Arrange
        DateTime birthDate = new DateTime(1990, 5, 15);
        string? nullCpr = null;

        // Act
        bool result = _cprService.ValidateCprWithDateOfBirth(nullCpr!, birthDate);

        // Assert
        Assert.False(result, "Null CPR should return false");
    }

    /// <summary>
    /// WHITE-BOX TEST: Tests the "empty CPR" path
    /// 
    /// What we're testing:
    /// - The first condition: string.IsNullOrEmpty(cprNumber)
    /// - Should return false for empty string
    /// </summary>
    [Fact]
    public void ValidateCprWithDateOfBirth_WithEmptyCpr_ShouldReturnFalse()
    {
        // Arrange
        DateTime birthDate = new DateTime(1990, 5, 15);
        string emptyCpr = "";

        // Act
        bool result = _cprService.ValidateCprWithDateOfBirth(emptyCpr, birthDate);

        // Assert
        Assert.False(result, "Empty CPR should return false");
    }

    /// <summary>
    /// WHITE-BOX TEST: Tests the "wrong length" path
    /// 
    /// What we're testing:
    /// - The length check: cprNumber.Length != 10
    /// - Should return false for CPR with wrong length
    /// </summary>
    [Fact]
    public void ValidateCprWithDateOfBirth_WithWrongLength_ShouldReturnFalse()
    {
        // Arrange
        DateTime birthDate = new DateTime(1990, 5, 15);
        string shortCpr = "150590123"; // Only 9 digits

        // Act
        bool result = _cprService.ValidateCprWithDateOfBirth(shortCpr, birthDate);

        // Assert
        Assert.False(result, "CPR with wrong length should return false");
    }

    /// <summary>
    /// WHITE-BOX TEST: Tests the "date mismatch" path
    /// 
    /// What we're testing:
    /// - When CPR date part doesn't match the birth date
    /// - The comparison: cprDatePart == expectedDatePart
    /// </summary>
    [Fact]
    public void ValidateCprWithDateOfBirth_WithMismatchedDate_ShouldReturnFalse()
    {
        // Arrange
        DateTime birthDate = new DateTime(1990, 5, 15);
        string wrongDateCpr = "1605901234"; // 16th (wrong day) May 1990

        // Act
        bool result = _cprService.ValidateCprWithDateOfBirth(wrongDateCpr, birthDate);

        // Assert
        Assert.False(result, "CPR with mismatched date should return false");
    }

    #endregion

    #region GenerateRandomCpr Tests - Testing random generation

    /// <summary>
    /// WHITE-BOX TEST: Tests that GenerateRandomCpr produces valid format
    /// 
    /// What we're testing:
    /// - The method creates a valid 10-digit CPR
    /// - The date part can be parsed and is valid
    /// - The last digit is either even or odd (valid for male/female)
    /// </summary>
    [Fact]
    public void GenerateRandomCpr_ShouldReturnValidFormat()
    {
        // Act: Generate a random CPR
        Cpr result = _cprService.GenerateRandomCpr();

        // Assert: Check basic format requirements
        Assert.NotNull(result);
        Assert.NotNull(result.Number);
        Assert.Equal(10, result.Number.Length);
        
        // Verify all characters are digits
        Assert.All(result.Number, c => Assert.True(char.IsDigit(c)));
        
        // Verify the CPR matches its own date of birth
        bool isValid = _cprService.ValidateCprWithDateOfBirth(result.Number, result.DateOfBirth);
        Assert.True(isValid, "Generated CPR should be valid for its own birth date");
    }

    /// <summary>
    /// WHITE-BOX TEST: Tests that multiple random CPRs are generated
    /// 
    /// What we're testing:
    /// - Random generation works consistently
    /// - Multiple calls don't fail
    /// - Each result follows the same rules
    /// </summary>
    [Fact]
    public void GenerateRandomCpr_MultipleCalls_ShouldAllBeValid()
    {
        // Act: Generate multiple CPRs
        for (int i = 0; i < 10; i++)
        {
            Cpr result = _cprService.GenerateRandomCpr();

            // Assert: Each one should be valid
            Assert.Equal(10, result.Number.Length);
            Assert.True(_cprService.ValidateCprWithDateOfBirth(result.Number, result.DateOfBirth));
        }
    }

    #endregion
}
