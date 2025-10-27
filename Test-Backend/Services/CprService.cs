using Test_Backend.Models;
using Test_Backend.Interfaces;

namespace Test_Backend.Services;

public class CprService : ICprService
{
    private readonly Random _random;

    public CprService()
    {
        _random = new Random();
    }

    public Cpr GenerateCpr(DateTime dateOfBirth, string gender)
    {
        string datePart = dateOfBirth.ToString("ddMMyy");

        int firstThreeDigits = _random.Next(0, 1000);

        int lastDigit = GenerateLastDigitByGender(gender);

        string cprNumber = $"{datePart}{firstThreeDigits:D3}{lastDigit}";

        return new Cpr
        {
            Number = cprNumber,
            DateOfBirth = dateOfBirth
        };
    }

    public Cpr GenerateRandomCpr()
    {
        DateTime randomDate = GenerateRandomDateOfBirth();

        string randomGender = _random.Next(2) == 0 ? "female" : "male";

        return GenerateCpr(randomDate, randomGender);
    }

    private int GenerateLastDigitByGender(string gender)
    {
        if (gender.Equals("female", StringComparison.OrdinalIgnoreCase))
        {
            return _random.Next(0, 5) * 2;
        }
        else
        {
            return _random.Next(0, 5) * 2 + 1;
        }
    }

    private DateTime GenerateRandomDateOfBirth()
    {
        DateTime start = new DateTime(1920, 1, 1);
        DateTime end = new DateTime(2024, 12, 31);

        int range = (end - start).Days;
        return start.AddDays(_random.Next(range));
    }

    public bool ValidateCprWithDateOfBirth(string cprNumber, DateTime dateOfBirth)
    {
        if (string.IsNullOrEmpty(cprNumber) || cprNumber.Length != 10)
            return false;

        string cprDatePart = cprNumber.Substring(0, 6);
        string expectedDatePart = dateOfBirth.ToString("ddMMyy");

        return cprDatePart == expectedDatePart;
    }
}
