namespace WarehouseManagmentAPI.Database.DatabaseModels
{
    public class ProductModel
    {
        public string SKU { get; set; }
        public string Nazwa_produktu { get; set; }
        public string Parametry { get; set; }
        public int Stan_magazynowy { get; set; }
        public int Potrzebna_ilosc { get; set; }
        public CartonModel Karton { get; set; }

    }
}
