using Microsoft.AspNetCore.Mvc;
using WarehouseManagmentAPI.Database.DatabaseControllers;
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
            var orders = OrderDbC.GetOrders();

            return Ok(orders);
        }

        [HttpGet("{id}")]
        public ActionResult<OrderModel> GetOrders(int id)
        {
            OrderModel order = OrderDbC.GetOrder(id);

            return Ok(order);
        }
    }
}
