using Test_Backend.Models;

namespace Test_Backend.Interfaces;

public interface INameService
{
    Name GetRandomName();
    IEnumerable<Name> GetNames();
    IEnumerable<Name> GetByGender(string gender);
}