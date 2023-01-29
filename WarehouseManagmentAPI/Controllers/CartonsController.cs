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
    public class CartonsController : ControllerBase
    {
        //Zwraca liste kartonów
        [HttpGet]
        public ActionResult<IEnumerable<CartonModel>> GetCartons()
        {
            var cartons = CartonDbC.GetCartons();

            return Ok(cartons);
        }

        //Zwraca karton o podanym id
        [HttpGet("{id}")]
        public ActionResult<IEnumerable<CartonModel>> GetCarton(string id)
        {
            var carton = CartonDbC.GetCarton(id);

            return Ok(carton);
        }
    }
}
