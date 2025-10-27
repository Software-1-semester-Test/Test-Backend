using MySql.Data.MySqlClient;
using Test_Backend.Models;

namespace Test_Backend.Services;

public class AddressService
{
    private readonly string? _connectionString;
    private static readonly Random _rand= new Random();

    public AddressService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public AddressService()
    {
        
    }

    public Adress GetRandomAddress()
    {
        var address = new Adress
        {
            Street = RandomStreet(),
            Number = RandomNumber(),
            Floor = RandomFloor(),
            Door = RandomDoor()
        };

        // Get random postal code and town from DB
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        using var cmd = new MySqlCommand(
            "SELECT cPostalCode, cTownName FROM postal_code ORDER BY RAND() LIMIT 1;", connection);
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            address.PostalCode = reader.GetString("cPostalCode");
            address.Town = reader.GetString("cTownName");
        }

        return address;
    }

    public string RandomStreet()
    {
        int length = _rand.Next(5, 12);
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        return new string(Enumerable.Range(0, length).Select(_ => chars[_rand.Next(chars.Length)]).ToArray()) + "vej";
    }

    public string RandomNumber()
    {
        int number = _rand.Next(1, 1000);
        string letter = _rand.NextDouble() < 0.3 ? ((char)_rand.Next('A', 'Z' + 1)).ToString() : "";
        return $"{number}{letter}";
    }

    public string RandomFloor()
    {
        return _rand.NextDouble() < 0.2 ? "st" : _rand.Next(1, 100).ToString();
    }

    public string RandomDoor()
    {
        string[] fixedDoors = { "th", "mf", "tv" };
        if (_rand.NextDouble() < 0.3)
            return fixedDoors[_rand.Next(fixedDoors.Length)];

        string letter = ((char)_rand.Next('a', 'z' + 1)).ToString();
        string dash = _rand.NextDouble() < 0.5 ? "-" : "";
        int number = _rand.Next(1, 1000);
        return $"{letter}{dash}{number}";
    }
}
