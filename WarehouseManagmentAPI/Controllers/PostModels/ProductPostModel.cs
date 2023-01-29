using System.ComponentModel.DataAnnotations;

namespace WarehouseManagmentAPI.Models
{
    //Wstępne utworzenie modelu z wymaganymi polami => jeszcze nie obsługuje po stronie web
    public class ProductPostModel
    {
        [Required]
        [MaxLength(25)]
        public string SKU { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int CartonID { get; set; }
    }
}
