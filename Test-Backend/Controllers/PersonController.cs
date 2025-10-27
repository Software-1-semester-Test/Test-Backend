using Microsoft.AspNetCore.Mvc;
using Test_Backend.Models;
using Test_Backend.Interfaces;

namespace Test_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController(IPersonService personService) : ControllerBase
    {
        /// <summary>
        /// Get a single random person
        /// </summary>
        [HttpGet]
        public ActionResult<Person> GetRandomPerson()
        {
            var person = personService.GetRandomPerson();
            return Ok(person);
        }

        /// <summary>
        /// Get multiple random people
        /// Example: /api/person/bulk?count=10
        /// </summary>
        [HttpGet("bulk")]
        public ActionResult<List<Person>> GetRandomPersons([FromQuery] int count = 2)
        {
            var people = personService.GetRandomPersons(count);
            return Ok(people);
        }
    }
}