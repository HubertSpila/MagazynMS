using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagmentAPI.Controllers.PostModels;
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
        public ActionResult<IEnumerable<CartonModel>> GetCarton(int id)
        {
            var carton = CartonDbC.GetCarton(id);

            return Ok(carton);
        }

        [HttpPut("update")]
        public ActionResult<IEnumerable<CartonModel>> UpdateCarton(ChangeCartonQuantityPostModel form)
        {
            CartonDbC.UpdateCarton(form);
            return Ok();
        }

        [HttpPost("add")]
        public ActionResult<IEnumerable<CartonModel>> AddCarton(AddCartonPostModels form)
        {
            CartonDbC.AddCarton(form);
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public ActionResult<IEnumerable<CartonModel>> DeleteCarton(int id)
        {
            CartonDbC.DeleteCarton(id);
            return Ok();
        }
    }
}
