using Test_Backend.Models;

namespace Test_Backend.Services;

public class CprService
{
    private readonly Random _random;

    public CprService()
    {
        _random = new Random();
    }

    /// <summary>
    /// Genererer et CPR nummer baseret på fødselsdato og køn
    /// De første 6 cifre: ddMMyy (fra fødselsdato)
    /// De sidste 4 cifre: XXXC hvor C er lige for kvinder, ulige for mænd
    /// </summary>
    /// <param name="dateOfBirth">Fødselsdato</param>
    /// <param name="gender">Køn: "male" eller "female"</param>
    /// <returns>CPR objekt med nummer og fødselsdato</returns>
    public Cpr GenerateCpr(DateTime dateOfBirth, string gender)
    {
        // Første 6 cifre: ddMMyy fra fødselsdato
        string datePart = dateOfBirth.ToString("ddMMyy");

        // Generer de første 3 tilfældige cifre (000-999)
        int firstThreeDigits = _random.Next(0, 1000);

        // Sidste ciffer baseret på køn
        int lastDigit = GenerateLastDigitByGender(gender);

        // Sammensæt CPR nummer: ddMMyyXXXC
        string cprNumber = $"{datePart}{firstThreeDigits:D3}{lastDigit}";

        return new Cpr
        {
            Number = cprNumber,
            DateOfBirth = dateOfBirth
        };
    }

    /// <summary>
    /// Genererer et tilfældigt CPR nummer med tilfældig fødselsdato og køn
    /// </summary>
    public Cpr GenerateRandomCpr()
    {
        // Generer tilfældig fødselsdato (mellem 1920 og 2024)
        DateTime randomDate = GenerateRandomDateOfBirth();
        
        // Vælg tilfældigt køn
        string randomGender = _random.Next(2) == 0 ? "female" : "male";

        return GenerateCpr(randomDate, randomGender);
    }

    /// <summary>
    /// Genererer sidste ciffer baseret på køn
    /// Kvinder: lige tal (0, 2, 4, 6, 8)
    /// Mænd: ulige tal (1, 3, 5, 7, 9)
    /// </summary>
    private int GenerateLastDigitByGender(string gender)
    {
        if (gender.Equals("female", StringComparison.OrdinalIgnoreCase))
        {
            // Lige tal for kvinder
            return _random.Next(0, 5) * 2; // 0, 2, 4, 6, 8
        }
        else // male
        {
            // Ulige tal for mænd
            return _random.Next(0, 5) * 2 + 1; // 1, 3, 5, 7, 9
        }
    }

    /// <summary>
    /// Genererer tilfældig fødselsdato mellem 1920 og 2024
    /// </summary>
    private DateTime GenerateRandomDateOfBirth()
    {
        DateTime start = new DateTime(1920, 1, 1);
        DateTime end = new DateTime(2024, 12, 31);
        
        int range = (end - start).Days;
        return start.AddDays(_random.Next(range));
    }

    /// <summary>
    /// Validerer om CPR nummer matcher fødselsdatoen
    /// </summary>
    public bool ValidateCprWithDateOfBirth(string cprNumber, DateTime dateOfBirth)
    {
        if (string.IsNullOrEmpty(cprNumber) || cprNumber.Length != 10)
            return false;

        // Udtræk dato delen fra CPR (første 6 cifre)
        string cprDatePart = cprNumber.Substring(0, 6);
        string expectedDatePart = dateOfBirth.ToString("ddMMyy");

        return cprDatePart == expectedDatePart;
    }

   
}
