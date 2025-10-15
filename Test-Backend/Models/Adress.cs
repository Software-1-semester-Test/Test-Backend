namespace Test_Backend.Models;

public class Adress
{
    public string Street { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Floor { get; set; } = string.Empty;
    public string Door { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Town { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"{Street} {Number}, {Floor}. {Door}, {PostalCode} {Town}";
    }
}