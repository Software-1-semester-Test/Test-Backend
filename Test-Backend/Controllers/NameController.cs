using Microsoft.AspNetCore.Mvc;
using Test_Backend.Services;
using Test_Backend.Interfaces;

namespace Test_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NameController(INameService nameService) : ControllerBase
    {
        private readonly Random _random = new();

        // --- /api/name/random ---
        [HttpGet("random")]
        public IActionResult GetRandom([FromQuery] bool includeDob = false)
        {
            var name = nameService.GetRandomName();

            if (includeDob)
            {
                // generate random DOB between 1950 and 2010
                var start = new DateTime(1950, 1, 1);
                var end = new DateTime(2010, 12, 31);
                var dob = start.AddDays(_random.Next((end - start).Days));

                return Ok(new
                {
                    name = name.FirstName,
                    surname = name.LastName,
                    gender = name.Gender,
                    dateOfBirth = dob
                });
            }

            // default (no DOB)
            return Ok(new
            {
                name = name.FirstName,
                surname = name.LastName,
                gender = name.Gender
            });
        }

        // --- /api/name/all ---
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            return Ok(nameService.GetNames());
        }

        // --- /api/name/gender/{gender} ---
        [HttpGet("gender/{gender}")]
        public IActionResult GetByGender(string gender)
        {
            var results = nameService.GetByGender(gender);
            return Ok(results);
        }
    }
}