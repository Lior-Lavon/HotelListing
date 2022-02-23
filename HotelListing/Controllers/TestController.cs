using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> TestGet()
        {
            _logger.LogInformation("Test API - Get !!!");
            return StatusCode(200, "Test API - Get !!!");
        }

        [HttpPost]
        public async Task<IActionResult> TestPost()
        {
            _logger.LogInformation("Test API - Post !!!");
            return StatusCode(200, "Test API - Post !!!");
        }

        [HttpPut]
        public async Task<IActionResult> TestPut()
        {
            _logger.LogInformation("Test API - Put !!!");
            return StatusCode(200, "Test API - Put !!!");
        }

        [HttpDelete]
        public async Task<IActionResult> TestDelete()
        {
            _logger.LogInformation("Test API - Delete !!!");
            return StatusCode(200, "Test API - Delete !!!");
        }
    }
}
