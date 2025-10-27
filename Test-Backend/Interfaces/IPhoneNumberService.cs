using Test_Backend.Models;

namespace Test_Backend.Interfaces;

public interface IPhoneNumberService
{
    PhoneNumber GetRandomPhoneNumber();
}