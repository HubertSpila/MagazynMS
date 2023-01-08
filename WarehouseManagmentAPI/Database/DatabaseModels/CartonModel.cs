namespace WarehouseManagmentAPI.Database.DatabaseModels
{
    public class CartonModel
    {
        public int ID_kartonu { get; set; }
        public int Wysokosc { get; set; }
        public int Szerokosc { get; set; }
        public int Glebokosc { get; set; }
        public int Stan_magazynowy { get; set; }
        public int Potrzebna_ilosc { get; set; }
    }
}
