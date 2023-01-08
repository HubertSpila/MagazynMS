using Microsoft.AspNetCore.Mvc;
using WarehouseManagmentAPI.Database.DatabaseControllers;

namespace WarehouseManagmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartonsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var t = CartonDbC.GetCartons();

            return null;
        }
    }
}
