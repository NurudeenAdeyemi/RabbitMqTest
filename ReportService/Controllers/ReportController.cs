using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rabbit.Models;

namespace ReportService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IBus _bus;
        public ReportController(IBus bus)
        {
            _bus = bus;
        }

        [HttpGet]
        public async Task<IActionResult> SendReport()
        {
            var message = new StudentReport
            {
                StudentNumber = "Bolanle", 
                Provider = "email",
                Target = "Test"
            };
            await _bus.Publish(message);
            return Ok(message);
        }

        [HttpGet("test")]
        public async Task<IActionResult> SendReport1(bool isPublic)
        {
            var message = new Report
            {
                StudentNumber = "Bolanle",
                Provider = "email",
                Target = "Test",
                IsPublic = isPublic
            };
            await _bus.Publish(message);
            return Ok(message);
        }
    }
}
