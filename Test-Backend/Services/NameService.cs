using System.ComponentModel;
using System.Text.Json;
using Test_Backend.Models;

namespace Test_Backend.Services;

public class NameService
{
    public List<Name> _names;
    public Random _random;

    public NameService(IWebHostEnvironment environment)
    {
        var filePath = Path.Combine(environment.ContentRootPath, "Data", "person-names.json");
        var json = File.ReadAllText(filePath);
        _names = JsonSerializer.Deserialize<List<Name>>(json);
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
