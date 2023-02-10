using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagmentAPI.Database.DatabaseControllers;
using WarehouseManagmentAPI.Database.DatabaseModels;

namespace WarehouseManagmentAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        //Zwraca liste zamówień
        [HttpGet]
        public ActionResult<IEnumerable<OrderModel>> GetOrders()
        {
            List<OrderModel> orders = OrderDbC.GetOrders();

            return Ok(orders);
        }

        //Zwraca zamówienie o podanym id
        [HttpGet("{id}")]
        public ActionResult<OrderModel> GetOrders(int id)
        {
            OrderModel order = OrderDbC.GetOrder(id);

            return Ok(order);
        }

        //filtrowanie
        [HttpGet("filtr")]
        public ActionResult<IEnumerable<OrderModel>> FiltrOrders()
        {
            var orders = OrderDbC.GetOrders().Where(x=>x.Pozycje
                                               .Where(y=>y.Ilosc>1).Any());

            return Ok(orders);
        }
    }
}
