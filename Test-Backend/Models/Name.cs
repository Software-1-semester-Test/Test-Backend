using System.Text.Json.Serialization;

namespace Test_Backend.Models;

public class Name
{
    [JsonPropertyName("name")]
    public string FirstName { get; set; }
    
    [JsonPropertyName("surname")]
    public string LastName { get; set; }
    
    [JsonPropertyName("gender")]
    public string? Gender { get; set; }
}