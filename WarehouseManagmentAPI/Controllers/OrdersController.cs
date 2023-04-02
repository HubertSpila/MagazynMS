using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagmentAPI.Controllers.PostModels;
using WarehouseManagmentAPI.Database.DatabaseControllers;
using WarehouseManagmentAPI.Database.DatabaseModels;
using WarehouseManagmentAPI.Tools;

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
            var orders = OrderDbC.GetOrders().Where(x => x.Pozycje
                                               .Where(y => y.Ilosc > 1).Any());

            return Ok(orders);
        }

        //import
        [HttpGet("import")]
        public ActionResult<string> ImportOrders()
        {
            string path = @"C:\Users\Hubert\Desktop\Praca inżynierska\import";
            string[] files = Directory.GetFiles(path, "*.csv");

            OrderDbC.ClearOrders();
            List<OrderModel> list = new List<OrderModel>();

            foreach (string file in files)
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line == string.Empty) continue;
                        if (line.ToLower().Contains("\"status\"")) continue;

                        var order = ImportTools.ReturnOrder(line);
                        if (order != null && !list.Any(x=>x.ID_zamowienia == order.ID_zamowienia))
                        {
                            list.Add(order);
                        }
                    }
                }

                //Tworzenie archiwum
            }

            if (list.Any())
            {
                OrderDbC.AddOrders(ImportTools.PoprawDane(list));
            }

            return Ok();
        }
        [HttpPut("update")]
        public ActionResult<string> UpdateCarton(UpdateCartonOrderPostModel form)
        {
            OrderDbC.UpdateCarton(form);
            return Ok();
        }
    }
}
