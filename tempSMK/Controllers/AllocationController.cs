using Microsoft.AspNetCore.Mvc;
using tempSMK.Models;

namespace tempSMK.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AllocationController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                message = "Allocation API is running and accessible",
                endpoint = "/api/allocation",
                method = "POST",
                timestamp = DateTime.UtcNow
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] AllocationRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request body cannot be null");
            }

            var response = new AllocationResponse
            {
                ReceivedJobs = request.Jobs?.Count ?? 0,
                ReceivedVehicles = request.Vehicles?.Count ?? 0,
                ReceivedDepots = request.Depots?.Count ?? 0,
                Jobs = request.Jobs ?? new List<Job>(),
                Vehicles = request.Vehicles ?? new List<Vehicle>(),
                Depots = request.Depots ?? new List<Depot>()
            };

            return Ok(response);
        }
    }
}
