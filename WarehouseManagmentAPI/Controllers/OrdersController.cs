using Microsoft.AspNetCore.Mvc;
using WarehouseManagmentAPI.Database.DatabaseModels;

namespace WarehouseManagmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<OrderModel>> GetOrders()
        {
            OrderModel order = new OrderModel();

            return Ok(order);
        }

        [HttpGet("{id}")]
        public ActionResult<OrderModel> GetOrders(int id)
        {
            OrderModel order = new OrderModel();

            return Ok(order);
        }
    }
}
