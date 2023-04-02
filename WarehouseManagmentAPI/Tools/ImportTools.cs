using WarehouseManagmentAPI.Controllers;
using WarehouseManagmentAPI.Database.DatabaseControllers;
using WarehouseManagmentAPI.Database.DatabaseModels;

namespace WarehouseManagmentAPI.Tools
{
    public static class ImportTools
    {
        public static OrderModel ReturnOrder(string linia)
        {
            OrderModel orderModel = new OrderModel();
            orderModel.Pozycje = new List<PositionModel>();
            try
            {
                string[] wartosci = linia.Split("\"\"", StringSplitOptions.RemoveEmptyEntries);

                if (wartosci.Count() < 2) return null;

                string[] zamowienieWartosci = wartosci[0].Split(';', StringSplitOptions.RemoveEmptyEntries);

                if (zamowienieWartosci.Count() != 3) return null;

                orderModel.ID_zamowienia = Int32.Parse(zamowienieWartosci[1].Replace("\"", string.Empty));

                for (int i = 1; i < wartosci.Count(); i++)
                {
                    if (wartosci[i] == ";") continue;
                    string[] pozycjaWartosci = wartosci[i].Split(';', StringSplitOptions.RemoveEmptyEntries);

                    if (pozycjaWartosci.Count() != 3) 
                        return null;

                    orderModel.Pozycje.Add(new PositionModel
                    {
                        SKU = pozycjaWartosci[2].Replace("\"", string.Empty),
                        Ilosc = Int32.Parse(pozycjaWartosci[0].Replace("\"", string.Empty)),
                        ID_zamowienia = orderModel.ID_zamowienia
                    });

                    ProductModel product = ProductDbC.GetProduct(orderModel.Pozycje.Last().SKU);

                    if(product.Karton != null)
                    {
                        orderModel.Pozycje.Last().Czy_na_stanie = product.Stan_magazynowy > 0;
                        orderModel.ID_kartonu = product.Karton.ID_kartonu;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return orderModel;
        }

        public static List<OrderModel> PoprawDane(List<OrderModel> list)
        {
            foreach (var order in list)
            {
                if(order.Pozycje.Count() > 1)
                {
                    order.ID_kartonu = 0;
                }

                else if (order.Pozycje.Count() == 1 && order.Pozycje.First().Ilosc > 2)
                {
                    order.ID_kartonu = 0;
                }
            }
            
            return list;
        }

        public static void WpiszStany(string sku, int ilosc)
        {
            List<OrderModel> orders = OrderDbC.GetOrders();
            int zmiana = 0;

            foreach (var order in orders)
            {
                if (!order.Pozycje.Any(x => x.SKU == sku)) continue;

                foreach (var pozycja in order.Pozycje)
                {
                    if (pozycja.SKU != sku) continue;

                    zmiana++;

                    if (ilosc == 0) pozycja.Czy_na_stanie = false;
                    else pozycja.Czy_na_stanie = true;
                }
            }

            if(zmiana != 0)
            {
                OrderDbC.ClearOrders();
                OrderDbC.AddOrders(orders);
            }
        }
    }
}
