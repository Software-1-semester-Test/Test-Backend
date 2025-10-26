using Test_Backend.Models;

namespace Test_Backend.Interfaces;

public interface IPersonService
{
    Person GetRandomPerson();
    List<Person> GetRandomPersons(int count);
}