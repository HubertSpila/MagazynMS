using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WarehouseManagmentAPI.Controllers.PostModels;
using WarehouseManagmentAPI.Database.DatabaseControllers;
using WarehouseManagmentAPI.Database.DatabaseModels;
using WarehouseManagmentAPI.Tools;

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
            products = CountingTools.EntryNeededQuantity(products);

            return Ok(products);
        }
        [HttpPost]
        public ActionResult AddProduct(AddProductPostModel form)
        {
            ProductDbC.AddProduct(form);
            ImportTools.WpiszStany(form.SKU, form.Stan_magazynowy);

            return Ok();
        }
        [HttpGet]
        [Route("available")]
        public ActionResult<IEnumerable<ProductModel>> GetAvailableProducts()
        {
            List<ProductModel> products = ProductDbC.GetAvailableProducts();
            products = CountingTools.EntryNeededQuantity(products);

            return Ok(products);
        }

        [HttpGet("{sku}")]
        public ActionResult<ProductModel> GetProducts(string sku)
        {
            ProductModel product = ProductDbC.GetProduct(sku);

            return Ok(product);
        }

        [HttpPut("update")]
        public ActionResult<IEnumerable<ProductModel>> UpdateCarton(ChangeProductQuantityPostModel form)
        {
            ProductDbC.UpdateProduct(form);
            ImportTools.WpiszStany(form.sku, form.ilosc);

            return Ok();
        }
    }
}
