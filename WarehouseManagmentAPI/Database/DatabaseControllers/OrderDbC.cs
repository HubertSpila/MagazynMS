using Microsoft.Data.SqlClient;
using WarehouseManagmentAPI.Database.DatabaseModels;

namespace WarehouseManagmentAPI.Database.DatabaseControllers
{
    public static class OrderDbC
    {
        private static string _connectionString = @"Data Source=.\SQLExpress;Initial Catalog=MagazynMS;Trusted_Connection=True";
        public static List<OrderModel> GetOrders()
        {
            List<OrderModel> orders = new List<OrderModel>();

            using (SqlConnection Connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand($"SELECT * FROM Zamowienie", Connection);
                Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    orders.Add(new OrderModel()
                    {
                        ID_zamowienia = (int)reader[0],
                        ID_kartonu = (int)reader[1],
                        Czy_na_stanie = (bool)reader[2]
                    });
                }

                reader.Close();

                SqlCommand commandPossition = new SqlCommand($"SELECT * FROM Pozycja", Connection);
                SqlDataReader readerPosition = commandPossition.ExecuteReader();
                
                while (readerPosition.Read())
                {
                    PositionModel position = new PositionModel()
                    {
                        SKU = readerPosition[0].ToString(),
                        Ilosc = (int)readerPosition[1],
                        ID_zamowienia = (int)readerPosition[2]
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
    }
}
