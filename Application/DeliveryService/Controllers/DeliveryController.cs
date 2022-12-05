using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService deliveryService;
    }
}
