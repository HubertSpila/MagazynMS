using Microsoft.AspNetCore.Mvc;
using WarehouseManagmentAPI.Database.DatabaseModels;
using WarehouseManagmentAPI.Models;

namespace WarehouseManagmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        [HttpPost]
        public ActionResult<ProductModel> AddProduct(ProductPostModel productToAdd)
        {
            if (!ModelState.IsValid) return BadRequest();

            ProductModel newProduct = new ProductModel(productToAdd);

            return Ok(newProduct);
        }

        [HttpDelete("{sku}")]
        public ActionResult DeleteProduct(string sku)
        {
            
            return NoContent();
        }
    }
}
