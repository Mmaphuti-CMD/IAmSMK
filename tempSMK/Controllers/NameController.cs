using Microsoft.AspNetCore.Mvc;

namespace tempSMK.Controllers
{
    public class NameRequest
    {
        public string? Input { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class NameController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("API is working! Connected successfully.");
        }

        [HttpPost]
        public IActionResult Post([FromBody] NameRequest request)
        {
            if (request?.Input == "What is my name")
            {
                return Ok("I am SMK!");
            }
            
            return BadRequest("Hahaha!!!");
        }
    }
}

