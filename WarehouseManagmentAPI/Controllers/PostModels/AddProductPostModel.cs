namespace WarehouseManagmentAPI.Controllers.PostModels
{
    public class AddProductPostModel
    {
        public string SKU { get; set; }
        public string Nazwa_produktu { get; set; }
        public int ID_kartonu { get; set; }
        public int Stan_magazynowy { get; set; }
        public int Potrzebna_ilosc { get; set; }
        public string Parametry { get; set; }  
    }
}
