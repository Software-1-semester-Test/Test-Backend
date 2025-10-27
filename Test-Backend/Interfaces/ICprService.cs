using Test_Backend.Models;

namespace Test_Backend.Interfaces;

public interface ICprService
{
    Cpr GenerateCpr(DateTime dateOfBirth, string gender);
    Cpr GenerateRandomCpr();
    bool ValidateCprWithDateOfBirth(string cprNumber, DateTime dateOfBirth);
}