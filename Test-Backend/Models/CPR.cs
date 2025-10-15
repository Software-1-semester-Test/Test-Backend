using System.Runtime.Serialization;

namespace Test_Backend.Models;

public class Cpr
{
    public string Number { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    
    public override string ToString()
    {
        return Number;
    }
}