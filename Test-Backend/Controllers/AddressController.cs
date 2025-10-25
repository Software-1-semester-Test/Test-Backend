using Microsoft.AspNetCore.Mvc;
using System.Runtime.ConstrainedExecution;
using System.Xml.Linq;
using Test_Backend.Models;
using Test_Backend.Services;

namespace Test_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AddressController : ControllerBase
{
    private readonly AddressService _addressService;

    public AddressController(AddressService addressService)
    {
        _addressService = addressService;
    }

    [HttpGet("random")]
    public ActionResult<Adress> GetRandomAddress()
    {
        var address = _addressService.GetRandomAddress();
        var result = new
        {
            street = address.Street,
            doornumber = address.Number,
            floor = address.Floor,
            door = address.Door,
            postalCode = address.PostalCode,
            town = address.Town,

        };

        return Ok(result);
    }
}