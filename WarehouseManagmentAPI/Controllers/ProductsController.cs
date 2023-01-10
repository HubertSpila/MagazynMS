using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagmentAPI.Controllers.PostModels;
using WarehouseManagmentAPI.Database.DatabaseControllers;
using WarehouseManagmentAPI.Database.DatabaseModels;
using WarehouseManagmentAPI.Models;

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

        [HttpGet("{sku}")]
        public ActionResult<ProductModel> GetProducts(string sku)
        {
            ProductModel product = ProductDbC.GetProduct(sku);

            return Ok(product);
        }

        //-----------------------------------------------
        [HttpDelete("{sku}")]
        public ActionResult DeleteProduct(string sku)
        {

            return NoContent();
        }

        [HttpPost]
        public ActionResult<ProductModel> AddProduct(ProductPostModel productToAdd)
        {
            if (!ModelState.IsValid) return BadRequest();

            ProductModel newProduct = new ProductModel(productToAdd);

            return Ok(newProduct);
        }
    }
}
