using Microsoft.AspNetCore.Mvc;
using WarehouseManagmentAPI.Database.DatabaseControllers;
using WarehouseManagmentAPI.Database.DatabaseModels;

namespace WarehouseManagmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<ProductModel>> GetStatistics()
        {
            List<StatisticModel> statistics = StatisticsDbC.GetStatistics();

            return Ok(statistics);
        }
    }
}
