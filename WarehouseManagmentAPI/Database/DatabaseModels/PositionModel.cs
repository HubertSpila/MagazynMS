namespace WarehouseManagmentAPI.Database.DatabaseModels
{
    public class PositionModel
    {
        public string SKU { get; set; }
        public int Ilosc { get; set; }
        public int ID_zamowienia { get; set; }
    }
}