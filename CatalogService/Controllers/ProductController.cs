using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rabbit.Models;

namespace CatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        
        private readonly IBus _bus;
        public ProductController(IBus bus)
        {
            _bus = bus;
        }
        [HttpGet]
        public async Task<IActionResult> SendMessage()
        {
            var message = new MessageTest();
            //Uri uri = new Uri("rabbitmq://localhost/messageQueue");
             await _bus.Publish(message);
            //var endPoint = await _bus.GetSendEndpoint(uri);
            //await endPoint.Send(message);
            return Ok(message);
        }
    }
}
