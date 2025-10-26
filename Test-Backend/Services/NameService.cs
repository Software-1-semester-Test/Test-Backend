using System.ComponentModel;
using System.Text.Json;
using Test_Backend.Models;
using Test_Backend.Interfaces;

namespace Test_Backend.Services;

public class NameService : INameService
{
    private readonly List<Name> _names;
    private readonly Random _random;

    public NameService(IWebHostEnvironment environment)
    {
        _random = new Random();
        var filePath = Path.Combine(environment.ContentRootPath, "person-names.json");
        var json = File.ReadAllText(filePath);
        var data = JsonSerializer.Deserialize<PersonNamesData>(json);
        _names = data?.Persons ?? new List<Name>();
    }

    public Name GetRandomName()
    {
        if (_names.Count == 0)
            throw new InvalidOperationException("No names found xd)");
        return _names[_random.Next(_names.Count)];
    }
    
    public IEnumerable<Name> GetNames() => _names;
    
    public IEnumerable<Name> GetByGender(string gender) => GetNames().Where(n => n.Gender == gender);
}