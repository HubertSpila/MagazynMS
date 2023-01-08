using Microsoft.AspNetCore.Mvc;
using WarehouseManagmentAPI.Database.DatabaseControllers;
using WarehouseManagmentAPI.Database.DatabaseModels;

namespace WarehouseManagmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigurationsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<ConfigurationModel> GetStatistics()
        {
            ConfigurationModel config = ConfigurationDbC.GetConfiguration(Config.User);

            return Ok(config);
        }
    }
}
