using WarehouseManagmentAPI.Database.DatabaseControllers;
using WarehouseManagmentAPI.Database.DatabaseModels;

namespace WarehouseManagmentAPI.Tools
{
    public static class CountingTools
    {
        public static List<CartonModel> EntryNeededQuantity(List<CartonModel> cartons)
        {
            var orders = OrderDbC.GetOrders();
            var groups = orders.GroupBy(x => x.ID_kartonu);

            foreach (var group in groups)
            {
                cartons.First(x => x.ID_kartonu == group.Key).Potrzebna_ilosc = group.Count();    
            }

            foreach (OrderModel order in orders.Where(x=>x.ID_kartonu2 != null))
            {
                cartons.First(x => x.ID_kartonu == order.ID_kartonu2).Potrzebna_ilosc += 1;
            }

            return cartons;
        }
    }
}
