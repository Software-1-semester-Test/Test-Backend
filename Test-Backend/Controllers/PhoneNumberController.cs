using Microsoft.AspNetCore.Mvc;
using Test_Backend.Models;
using Test_Backend.Interfaces;

namespace Test_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhoneNumberController(IPhoneNumberService phoneNumberService) : ControllerBase
    {
        // GET api/phonenumber/random
        [HttpGet("random")]
        public ActionResult<PhoneNumber> GetRandomPhoneNumber()
        {
            var phoneNumber = phoneNumberService.GetRandomPhoneNumber();
            return Ok(phoneNumber);
        }
    }
}