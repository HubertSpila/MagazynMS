using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagmentAPI.Database.DatabaseControllers;
using WarehouseManagmentAPI.Database.DatabaseModels;
using WarehouseManagmentAPI.Tools.Imports;

namespace WarehouseManagmentAPI.Controllers
{
    [Authorize]
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

        [HttpGet("import")]
        public ActionResult ImportOrders()
        {
            OrderDbC.DeleteOrders();
            OrderDbC.AddOrders();

            return Ok();
        }

        [HttpGet("filtr")]
        public ActionResult<IEnumerable<OrderModel>> FiltrOrders()
        {
            var orders = OrderDbC.GetOrders().Where(x=>x.Pozycje
                                               .Where(y=>y.Ilosc>1).Any());

            return Ok(orders);
        }
    }
}
