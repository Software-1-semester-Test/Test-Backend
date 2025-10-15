using Microsoft.AspNetCore.Mvc;
using Test_Backend.Models;
using Test_Backend.Services;

namespace Test_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CprController : ControllerBase
{
    private readonly CprService _cprService;

    public CprController(CprService cprService)
    {
        _cprService = cprService;
    }

    /// <summary>
    /// Genererer et tilf�ldigt CPR nummer med tilf�ldig f�dselsdato og k�n
    /// </summary>
    [HttpGet("random")]
    public ActionResult<Cpr> GetRandomCpr()
    {
        var cpr = _cprService.GenerateRandomCpr();
        return Ok(cpr);
    }

    /// <summary>
    /// Genererer et CPR nummer baseret p� specifik f�dselsdato og k�n
    /// </summary>
    /// <param name="dateOfBirth">F�dselsdato (format: yyyy-MM-dd)</param>
    /// <param name="gender">K�n: 'male' eller 'female'</param>
    [HttpPost("generate")]
    public ActionResult<Cpr> GenerateCpr([FromQuery] DateTime dateOfBirth, [FromQuery] string gender)
    {
        if (string.IsNullOrEmpty(gender))
            return BadRequest("Gender must be specified ('male' or 'female')");

        if (!gender.Equals("male", StringComparison.OrdinalIgnoreCase) && 
            !gender.Equals("female", StringComparison.OrdinalIgnoreCase))
            return BadRequest("Gender must be 'male' or 'female'");

        var cpr = _cprService.GenerateCpr(dateOfBirth, gender);
        return Ok(cpr);
    }

    
}
