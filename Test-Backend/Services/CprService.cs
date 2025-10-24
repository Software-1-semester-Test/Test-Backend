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
    /// Genererer et CPR nummer baseret p� f�dselsdato og k�n
    /// De f�rste 6 cifre: ddMMyy (fra f�dselsdato)
    /// De sidste 4 cifre: XXXC hvor C er lige for kvinder, ulige for m�nd
    /// </summary>
    /// <param name="dateOfBirth">F�dselsdato</param>
    /// <param name="gender">K�n: "male" eller "female"</param>
    /// <returns>CPR objekt med nummer og f�dselsdato</returns>
    public Cpr GenerateCpr(DateTime dateOfBirth, string gender)
    {
        // F�rste 6 cifre: ddMMyy fra f�dselsdato
        string datePart = dateOfBirth.ToString("ddMMyy");

        // Generer de f�rste 3 tilf�ldige cifre (000-999)
        int firstThreeDigits = _random.Next(0, 1000);

        // Sidste ciffer baseret p� k�n
        int lastDigit = GenerateLastDigitByGender(gender);

        // Sammens�t CPR nummer: ddMMyyXXXC
        string cprNumber = $"{datePart}{firstThreeDigits:D3}{lastDigit}";

        return new Cpr
        {
            Number = cprNumber,
            DateOfBirth = dateOfBirth
        };
    }

    /// <summary>
    /// Genererer et tilf�ldigt CPR nummer med tilf�ldig f�dselsdato og k�n
    /// </summary>
    public Cpr GenerateRandomCpr()
    {
        // Generer tilf�ldig f�dselsdato (mellem 1920 og 2024)
        DateTime randomDate = GenerateRandomDateOfBirth();
        
        // V�lg tilf�ldigt k�n
        string randomGender = _random.Next(2) == 0 ? "female" : "male";

        return GenerateCpr(randomDate, randomGender);
    }

    /// <summary>
    /// Genererer sidste ciffer baseret p� k�n
    /// Kvinder: lige tal (0, 2, 4, 6, 8)
    /// M�nd: ulige tal (1, 3, 5, 7, 9)
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
            // Ulige tal for m�nd
            return _random.Next(0, 5) * 2 + 1; // 1, 3, 5, 7, 9
        }
    }

    /// <summary>
    /// Genererer tilf�ldig f�dselsdato mellem 1920 og 2024
    /// </summary>
    private DateTime GenerateRandomDateOfBirth()
    {
        DateTime start = new DateTime(1920, 1, 1);
        DateTime end = new DateTime(2024, 12, 31);
        
        int range = (end - start).Days;
        return start.AddDays(_random.Next(range));
    }

    /// <summary>
    /// Validerer om CPR nummer matcher f�dselsdatoen
    /// </summary>
    public bool ValidateCprWithDateOfBirth(string cprNumber, DateTime dateOfBirth)
    {
        if (string.IsNullOrEmpty(cprNumber) || cprNumber.Length != 10)
            return false;

        // Udtr�k dato delen fra CPR (f�rste 6 cifre)
        string cprDatePart = cprNumber.Substring(0, 6);
        string expectedDatePart = dateOfBirth.ToString("ddMMyy");

        return cprDatePart == expectedDatePart;
    }

   
}
