using Microsoft.AspNetCore.Mvc;
using WarehouseManagmentAPI.Database.DatabaseControllers;
using WarehouseManagmentAPI.Database.DatabaseModels;
using WarehouseManagmentAPI.Imports;

namespace WarehouseManagmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartonsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<CartonModel>> GetCartons()
        {
            var cartons = CartonDbC.GetCartons();

            return Ok(cartons);
        }

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<CartonModel>> GetCarton(string id)
        {
            var carton = CartonDbC.GetCarton(id);

            return Ok(carton);
        }
    }
}
