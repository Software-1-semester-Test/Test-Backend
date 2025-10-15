using Microsoft.AspNetCore.Mvc;
using Test_Backend.Services;
using Test_Backend.Models;

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
        return Ok(address);
    }
}