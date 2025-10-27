using Test_Backend.Services;
using Test_Backend.Models;
using Test_Backend.Interfaces;
using NSubstitute;

namespace Test_Backend.Tests;

public class PersonServiceTests
{
    private readonly INameService _nameService;
    private readonly ICprService _cprService;
    private readonly IAddressService _addressService;
    private readonly IPhoneNumberService _phoneService;
    private readonly PersonService _personService;

    public PersonServiceTests()
    {
        _nameService = Substitute.For<INameService>();
        _cprService = Substitute.For<ICprService>();
        _addressService = Substitute.For<IAddressService>();
        _phoneService = Substitute.For<IPhoneNumberService>();
        _personService = new PersonService(_nameService, _cprService, _addressService, _phoneService);
    }

    [Fact]
    public void GetRandomPerson_WhenNameHasGender_RegeneratesCprWithMatchingGender()
    {
        var mockName = new Name { Gender = "Male", FirstName = "Jeppe" };
        var originalCpr = new Cpr { DateOfBirth = new DateTime(2018, 4, 8) };
        var genderMatchedCpr = new Cpr { DateOfBirth = new DateTime(2018, 4, 8) };
        var mockAddress = new Adress { Street = "Mock Street", Number = "55" };
        var mockPhone = new PhoneNumber { Number = "12345678" };

        _nameService.GetRandomName().Returns(mockName);
        _cprService.GenerateRandomCpr().Returns(originalCpr);
        _cprService.GenerateCpr(originalCpr.DateOfBirth, "Male").Returns(genderMatchedCpr);
        _addressService.GetRandomAddress().Returns(mockAddress);
        _phoneService.GetRandomPhoneNumber().Returns(mockPhone);

        var person = _personService.GetRandomPerson();

        _cprService.Received(1).GenerateCpr(originalCpr.DateOfBirth, "Male");
        Assert.Equal(genderMatchedCpr, person.Cpr);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void GetRandomPerson_WhenNameGenderIsNullOrEmpty_UsesOriginalCpr(string gender)
    {
        var mockName = new Name { Gender = gender, FirstName = "Jeppe" };
        var originalCpr = new Cpr { DateOfBirth = new DateTime(2018, 4, 8) };
        var mockAddress = new Adress { Street = "Mock Street", Number = "55" };
        var mockPhone = new PhoneNumber { Number = "12345678" };

        _nameService.GetRandomName().Returns(mockName);
        _cprService.GenerateRandomCpr().Returns(originalCpr);
        _addressService.GetRandomAddress().Returns(mockAddress);
        _phoneService.GetRandomPhoneNumber().Returns(mockPhone);

        var person = _personService.GetRandomPerson();

        _cprService.DidNotReceive().GenerateCpr(Arg.Any<DateTime>(), Arg.Any<string>());
        Assert.Equal(originalCpr, person.Cpr);
    }

    [Theory]
    [InlineData(-5, 2)]
    [InlineData(0, 2)]
    [InlineData(1, 2)]
    [InlineData(2, 2)]
    [InlineData(50, 50)]
    [InlineData(100, 100)]
    [InlineData(101, 100)]
    public void GetRandomPersons_ClampsCountToValidRange(int input, int expected)
    {
        _nameService.GetRandomName().Returns(new Name { FirstName = "Test" });
        _cprService.GenerateRandomCpr().Returns(new Cpr());
        _addressService.GetRandomAddress().Returns(new Adress());
        _phoneService.GetRandomPhoneNumber().Returns(new PhoneNumber { Number = "12345678" });

        var persons = _personService.GetRandomPersons(input);

        Assert.Equal(expected, persons.Count);
    }

    [Fact]
    public void GetRandomPerson_CallsAllDependenciesOnce()
    {
        var mockName = new Name { Gender = "Female" };
        var mockCpr = new Cpr { DateOfBirth = DateTime.Now };
        var mockAddress = new Adress();
        var mockPhone = new PhoneNumber { Number = "11223344" };

        _nameService.GetRandomName().Returns(mockName);
        _cprService.GenerateRandomCpr().Returns(mockCpr);
        _cprService.GenerateCpr(Arg.Any<DateTime>(), Arg.Any<string>()).Returns(mockCpr);
        _addressService.GetRandomAddress().Returns(mockAddress);
        _phoneService.GetRandomPhoneNumber().Returns(mockPhone);

        var person = _personService.GetRandomPerson();

        _nameService.Received(1).GetRandomName();
        _cprService.Received(1).GenerateRandomCpr();
        _addressService.Received(1).GetRandomAddress();
        _phoneService.Received(1).GetRandomPhoneNumber();
    }

    [Theory]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(100)]
    public void GetRandomPersons_CreatesExactNumberOfPersons(int count)
    {
        _nameService.GetRandomName().Returns(new Name());
        _cprService.GenerateRandomCpr().Returns(new Cpr());
        _addressService.GetRandomAddress().Returns(new Adress());
        _phoneService.GetRandomPhoneNumber().Returns(new PhoneNumber { Number = "12345678" });

        var persons = _personService.GetRandomPersons(count);

        Assert.Equal(count, persons.Count);
        Assert.All(persons, p => Assert.NotNull(p));
    }
}