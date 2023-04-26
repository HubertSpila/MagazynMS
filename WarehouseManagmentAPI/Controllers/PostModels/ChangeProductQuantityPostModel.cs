namespace WarehouseManagmentAPI.Controllers.PostModels
{
    public class ChangeProductQuantityPostModel
    {
        public string sku { get; set; }
        public string parametry { get; set; }
        public int ilosc { get; set; }
    }
}
