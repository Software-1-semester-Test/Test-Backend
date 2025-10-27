using Test_Backend.Models;
using Test_Backend.Interfaces;

namespace Test_Backend.Services;

public class PersonService : IPersonService
{
    private readonly NameService _nameService;
    private readonly CprService _cprService;
    private readonly AddressService _addressService;
    private readonly PhoneNumberService _phoneService;

    public PersonService(
        NameService nameService,
        CprService cprService,
        AddressService addressService,
        PhoneNumberService phoneService)
    {
        _nameService = nameService;
        _cprService = cprService;
        _addressService = addressService;
        _phoneService = phoneService;
    }

    public Person GetRandomPerson()
    {
        var name = _nameService.GetRandomName();
        var cpr = _cprService.GenerateRandomCpr(); // random date & gender
        var address = _addressService.GetRandomAddress();
        var phone = _phoneService.GetRandomPhoneNumber();

        // Ensure CPR gender matches name gender
        if (!string.IsNullOrEmpty(name.Gender))
        {
            cpr = _cprService.GenerateCpr(cpr.DateOfBirth, name.Gender);
        }

        return new Person
        {
            Name = name,
            Cpr = cpr,
            Address = address,
            Mobile = phone
        };
    }

    public List<Person> GetRandomPersons(int count)
    {
        count = Math.Clamp(count, 2, 100); // only 2-100 allowed
        var list = new List<Person>();
        for (int i = 0; i < count; i++)
        {
            list.Add(GetRandomPerson());
        }
        return list;
    }
}