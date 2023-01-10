using System.ComponentModel.DataAnnotations;

namespace WarehouseManagmentAPI.Models
{
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
