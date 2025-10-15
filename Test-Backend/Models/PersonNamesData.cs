using System.Text.Json.Serialization;

namespace Test_Backend.Models;

public class PersonNamesData
{
    [JsonPropertyName("persons")]
    public List<Name> Persons { get; set; } = new();
}
