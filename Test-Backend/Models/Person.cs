namespace Test_Backend.Models;

public class Person
{
    public Cpr Cpr { get; set; } = new Cpr();
    public Name Name { get; set; } = new Name();
    public Adress Address { get; set; } = new Adress();
    public PhoneNumber Mobile { get; set; } = new PhoneNumber();
}