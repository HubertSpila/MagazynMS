namespace WarehouseManagmentAPI.Database.DatabaseModels
{
    public class OrderModel
    {
        public int ID_zamowienia { get; set; }
        public int ID_kartonu { get; set; }
        public bool Czy_na_stanie { get; set; }
        public List<PositionModel> Pozycje { get; set; }
    }
}