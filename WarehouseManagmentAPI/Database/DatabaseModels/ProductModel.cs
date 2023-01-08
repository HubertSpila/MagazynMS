using WarehouseManagmentAPI.Models;

namespace WarehouseManagmentAPI.Database.DatabaseModels
{
    public class ProductModel
    {
        public string SKU { get; set; }
        public string Nazwa_produktu { get; set; }
        public int ID_kartonu { get; set; }
        public int Stan_magazynowy { get; set; }
        public int Potrzebna_ilosc { get; set; }

        public ProductModel()
        {

        }
        public ProductModel(ProductPostModel productToAdd)
        {
            SKU = productToAdd.SKU;
            Nazwa_produktu = productToAdd.Name;
            ID_kartonu = productToAdd.CartonID;

            //spr zamówienia
        }
    }

}
