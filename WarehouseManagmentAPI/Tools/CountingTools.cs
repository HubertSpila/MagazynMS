using WarehouseManagmentAPI.Database.DatabaseControllers;
using WarehouseManagmentAPI.Database.DatabaseModels;

namespace WarehouseManagmentAPI.Tools
{
    public static class CountingTools
    {
        public static List<CartonModel> EntryNeededQuantity(List<CartonModel> cartons)
        {
            var orders = OrderDbC.GetOrders().Where(x=>x.ID_kartonu !=0);
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

        public static List<ProductModel> EntryNeededQuantity(List<ProductModel> products)
        {
            var positions = OrderDbC.GetOrders().SelectMany(x=>x.Pozycje);
            var groups = positions.GroupBy(x => x.SKU);

            foreach (var group in groups)
            {
                if (!products.Any(x => x.SKU == group.Key)) continue;

                products.First(x => x.SKU == group.Key).Potrzebna_ilosc = group.Sum(x=>x.Ilosc);
            }

            return products;
        }
    }
}
