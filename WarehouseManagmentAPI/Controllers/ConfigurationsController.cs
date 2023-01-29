using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagmentAPI.Database.DatabaseControllers;
using WarehouseManagmentAPI.Database.DatabaseModels;

namespace WarehouseManagmentAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigurationsController : ControllerBase
    {
        //Zwraca statystyki
        [HttpGet]
        public ActionResult<ConfigurationModel> GetStatistics()
        {
            ConfigurationModel config = ConfigurationDbC.GetConfiguration(Config.User);

            return Ok(config);
        }
    }
}
