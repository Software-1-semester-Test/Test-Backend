using Microsoft.AspNetCore.Mvc;
using Test_Backend.Models;
using Test_Backend.Services;
using Test_Backend.Interfaces;

namespace Test_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CprController(ICprService cprService, INameService nameService) : ControllerBase
{
    // ============================================================
    // 1️⃣ CPR ONLY 
    // ============================================================
    [HttpGet("random")]
    public ActionResult<Cpr> GetRandomCpr()
    {
        var cpr = cprService.GenerateRandomCpr();
        var cprObject= new Dictionary<string, object>
        {
            ["cprnummer"] = cpr.Number
        };
        return Ok(cprObject);
    }

    // ============================================================
    // 2️⃣ CPR + NAME + GENDER
    // ============================================================
    [HttpGet("random/with-name-gender")]
    public IActionResult GetCprWithNameGender()
    {
        var cpr = cprService.GenerateRandomCpr();
        var name = nameService.GetRandomName();

        var result = new
        {
            cprnummer = cpr.Number,
            name = new
            {
                name = name.FirstName,
                surname = name.LastName,
                gender = name.Gender
            }
        };

        return Ok(result);
    }

    // ============================================================
    // 3️⃣ CPR + NAME + GENDER + DOB 
    // ============================================================
    [HttpGet("random/with-name-gender-dob")]
    public IActionResult GetCprWithNameGenderDob()
    {
        var cpr = cprService.GenerateRandomCpr();
        var name = nameService.GetRandomName();

        var result = new
        {
            cpr = new
            {
                number = cpr.Number,
                dateOfBirth = cpr.DateOfBirth
            },
            name = new
            {
                name = name.FirstName,
                surname = name.LastName,
                gender = name.Gender
            },
            dateOfBirth = cpr.DateOfBirth
        };

        return Ok(result);
    }

    // ============================================================
    // 4️⃣ Manual CPR generation (for completeness)
    // ============================================================
    [HttpPost("generate")]
    public ActionResult<Cpr> GenerateCpr([FromQuery] DateTime dateOfBirth, [FromQuery] string gender)
    {
        if (string.IsNullOrEmpty(gender))
            return BadRequest("Gender must be specified ('male' or 'female')");

        if (!gender.Equals("male", StringComparison.OrdinalIgnoreCase) &&
            !gender.Equals("female", StringComparison.OrdinalIgnoreCase))
            return BadRequest("Gender must be 'male' or 'female'");

        var cpr = cprService.GenerateCpr(dateOfBirth, gender);
        return Ok(cpr);
    }
}
