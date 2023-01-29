using Microsoft.Data.SqlClient;
using WarehouseManagmentAPI.Database.DatabaseModels;
using WarehouseManagmentAPI.Tools.Imports;

namespace WarehouseManagmentAPI.Database.DatabaseControllers
{
    public static class OrderDbC
    {
        //pobieranie danych z bazy do modelu OrderModel (Lista)
        public static List<OrderModel> GetOrders()
        {
            List<OrderModel> orders = new List<OrderModel>();

            using (SqlConnection Connection = new SqlConnection(Config._connectionString))
            {
                //Zapytanie SQL
                SqlCommand command = new SqlCommand($"SELECT * FROM Zamowienie", Connection);

                Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                //Odczyt wierszy z SQL
                while (reader.Read())
                {
                    orders.Add(new OrderModel()
                    {
                        ID_zamowienia = (int)reader[0],
                        ID_kartonu = (int)reader[1],
                        Czy_na_stanie = reader[2].ToString() != "0",
                        Pozycje = new List<PositionModel>()
                    });
                }

                reader.Close();

                //Zapytanie SQL
                SqlCommand commandPossition = new SqlCommand($"SELECT * FROM Pozycja", Connection);

                SqlDataReader readerPosition = commandPossition.ExecuteReader();

                //Odczyt wierszy z SQL
                while (readerPosition.Read())
                {
                    PositionModel position = new PositionModel()
                    {
                        SKU = readerPosition[0].ToString(),
                        Ilosc = (int)readerPosition[1],
                        ID_zamowienia = (int)readerPosition[2],
                        Czy_na_stanie = readerPosition[3].ToString() != "0"
                    };

                    if (orders.Where(x => x.ID_zamowienia == position.ID_zamowienia).FirstOrDefault() == null)
                        throw new Exception($"Brak ZO {position.ID_zamowienia}");

                    orders.Where(x => x.ID_zamowienia == position.ID_zamowienia).First().Pozycje.Add(position);
                }

                readerPosition.Close();
                Connection.Close();
            }

            return orders;
        }

        //pobieranie danych z bazy do modelu OrderModel
        public static OrderModel GetOrder(int id)
        {
            OrderModel order = new OrderModel();

            using (SqlConnection Connection = new SqlConnection(Config._connectionString))
            {
                //Zapytanie SQL
                SqlCommand command = new SqlCommand($"SELECT * FROM Zamowienie WHERE ID_zamowienia = {id}", Connection);

                Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                //Odczyt wierszy z SQL
                while (reader.Read())
                {
                    order = new OrderModel()
                    {
                        ID_zamowienia = (int)reader[0],
                        ID_kartonu = (int)reader[1],
                        Czy_na_stanie = reader[2].ToString() == "0",
                        Pozycje = new List<PositionModel>()
                    };
                }

                reader.Close();

                if (order != null)
                {
                    SqlCommand commandPossition = new SqlCommand($"SELECT * FROM Pozycja WHERE ID_zamowienia = {id}", Connection);
                    SqlDataReader readerPosition = commandPossition.ExecuteReader();

                    //Odczyt wierszy z SQL
                    while (readerPosition.Read())
                    {
                        PositionModel position = new PositionModel()
                        {
                            SKU = readerPosition[0].ToString(),
                            Ilosc = (int)readerPosition[1],
                            ID_zamowienia = (int)readerPosition[2]
                        };

                        order.Pozycje.Add(position);
                    }

                    readerPosition.Close();
                }

                Connection.Close();
            }

            return order;
        }

        public static void DeleteOrders()
        {
            List<OrderModel> orders = GetOrders();

            foreach (var order in orders)
            {
                if (BaseLinkerImport.OrderImport(order.ID_zamowienia).order_status_id != Config.status_id)
                {
                    using (SqlConnection Connection = new SqlConnection(Config._connectionString))
                    {
                        SqlCommand command = new SqlCommand($"DELLETE FROM Zamowienie WHERE ID_zamowienia = {order.ID_zamowienia}", Connection);
                        Connection.Open();

                        SqlDataReader reader = command.ExecuteReader();
                        reader.Close();

                        SqlCommand commandPossition = new SqlCommand($"DELLETE FROM Pozycja WHERE ID_zamowienia = {order.ID_zamowienia}", Connection);

                        SqlDataReader readerPosition = commandPossition.ExecuteReader();
                        readerPosition.Close();

                        Connection.Close();
                    }
                }
            }
        }

        public static void AddOrders()
        {
            List<OrderModel> orders = GetOrders();
            var baseLinkerOrders = BaseLinkerImport.OrdersImport();
            
            foreach (var order in baseLinkerOrders)
            {
                if (!orders.Where(x=>x.ID_zamowienia == order.order_id).Any())
                {
                    using (SqlConnection Connection = new SqlConnection(Config._connectionString))
                    {
                        SqlCommand command = new SqlCommand($"INSERT INTO Zamowienie (ID_zamowienia, Czy_na_stanie) VALUES ({order.order_id}, {IsAvailable(order)})", Connection);
                        if(order.products.Count == 1)
                        {
                            var product = ProductDbC.GetProduct(order.products.First().sku);
                            
                            if(product != null)
                                command = new SqlCommand($"INSERT INTO Zamowienie (ID_zamowienia, ID_kartonu, Czy_na_stanie) VALUES ({order.order_id}, {product.ID_kartonu}, {IsAvailable(order)})", Connection);
                        }

                        Connection.Open();

                        SqlDataReader reader = command.ExecuteReader();
                        reader.Close();

                        foreach (var product in order.products)
                        {
                            SqlCommand commandProduct = new SqlCommand($"INSERT INTO Pozycja (SKU, Ilosc, ID_zamowienia) VALUES ({product.sku}, {product.quantity}, {order.order_id})", Connection);
                            SqlDataReader readerProduct = commandProduct.ExecuteReader();
                            readerProduct.Close();
                        }

                        Connection.Close();
                    }
                }
            }
        }

        public static bool IsAvailable(ResponsePage order)
        {
            var products = ProductDbC.GetAvailableProducts();
            
            return order.products.Where(x=> products
                                    .Where(y=>y.SKU == x.sku).Any())
                                .Any();
        }
    }
}
