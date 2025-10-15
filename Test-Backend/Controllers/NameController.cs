using Microsoft.AspNetCore.Mvc;
using Test_Backend.Services;
using Test_Backend.Models;

namespace Test_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NameController(NameService nameService) : ControllerBase
    {
        [HttpGet("random")]
        public ActionResult<Name> GetRandom()
        {
            return Ok(nameService.GetRandomName());
        }

        [HttpGet("all")]
        public ActionResult<IEnumerable<Name>> GetAll()
        {
            return Ok(nameService.GetNames());
        }

        [HttpGet("gender/{gender}")]
        public ActionResult<IEnumerable<Name>> GetByGender(string gender)
        {
            var results = nameService.GetByGender(gender);
            return Ok(results);
        }
    }
}