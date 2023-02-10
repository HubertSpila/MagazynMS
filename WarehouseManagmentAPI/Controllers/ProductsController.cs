using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagmentAPI.Controllers.PostModels;
using WarehouseManagmentAPI.Database.DatabaseControllers;
using WarehouseManagmentAPI.Database.DatabaseModels;

namespace WarehouseManagmentAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<ProductModel>> GetProducts()
        {
            List<ProductModel> products = ProductDbC.GetProducts();

            return Ok(products);
        }
        [HttpPost]
        public ActionResult AddProduct(AddProductPostModel form)
        {
            ProductDbC.AddProduct(form);

            return Ok();
        }
        [HttpGet]
        [Route("available")]
        public ActionResult<IEnumerable<ProductModel>> GetAvailableProducts()
        {
            List<ProductModel> products = ProductDbC.GetAvailableProducts();

            return Ok(products);
        }

        [HttpGet("{sku}")]
        public ActionResult<ProductModel> GetProducts(string sku)
        {
            ProductModel product = ProductDbC.GetProduct(sku);

            return Ok(product);
        }

    }
}
