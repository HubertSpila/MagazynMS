using WarehouseManagmentAPI.Controllers;
using WarehouseManagmentAPI.Database.DatabaseControllers;
using WarehouseManagmentAPI.Database.DatabaseModels;

namespace WarehouseManagmentAPI.Tools
{
    public static class CsvParser
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
                    string[] pozycjaWartosci = wartosci[i].Split(';', StringSplitOptions.RemoveEmptyEntries);

                    if (pozycjaWartosci.Count() != 3) return null;

                    orderModel.Pozycje.Add(new PositionModel
                    {
                        SKU = pozycjaWartosci[2].Replace("\"", string.Empty),
                        Ilosc = Int32.Parse(pozycjaWartosci[0].Replace("\"", string.Empty)),
                        ID_zamowienia = orderModel.ID_zamowienia
                    });

                    ProductModel product = ProductDbC.GetProduct(orderModel.Pozycje.Last().SKU);
                    orderModel.Pozycje.Last().Czy_na_stanie = product.Stan_magazynowy > 0;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return orderModel;
        }
    }
}
