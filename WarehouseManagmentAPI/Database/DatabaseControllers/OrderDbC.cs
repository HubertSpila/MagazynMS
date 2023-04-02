using Microsoft.Data.SqlClient;
using System.Collections.Specialized;
using WarehouseManagmentAPI.Controllers.PostModels;
using WarehouseManagmentAPI.Database.DatabaseModels;

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
                    OrderModel order = new OrderModel();

                    order.ID_zamowienia = (int)reader[0];
                    order.ID_kartonu = (int)reader[1];
                    order.Czy_na_stanie = reader[2].ToString() != "0";

                    try { order.ID_kartonu2 = (int)reader[3]; }
                    catch { order.ID_kartonu2 = null; }
                    
                    order.Pozycje = new List<PositionModel>();

                    orders.Add(order);
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
                    order.ID_zamowienia = (int)reader[0];
                    order.ID_kartonu = (int)reader[1];
                    order.Czy_na_stanie = reader[2].ToString() != "0";

                    try { order.ID_kartonu2 = (int)reader[3]; }
                    catch { order.ID_kartonu2 = null; }
                    
                    order.Pozycje = new List<PositionModel>();
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

        // czyszczenie zamówień
        public static bool ClearOrders()
        {
            try
            {

                using (SqlConnection Connection = new SqlConnection(Config._connectionString))
                {
                    //Zapytanie SQL
                    string deletePozycjaQuery = "DELETE FROM Pozycja";
                    string deleteZamowienieQuery = "DELETE FROM Zamowienie";

                    SqlCommand deletePozycjaCommand = new SqlCommand(deletePozycjaQuery, Connection);
                    SqlCommand deleteZamowienieCommand = new SqlCommand(deleteZamowienieQuery, Connection);

                    Connection.Open();

                    deletePozycjaCommand.ExecuteNonQuery();
                    deleteZamowienieCommand.ExecuteNonQuery();

                    Connection.Close();
                }
            }
            catch(Exception ex)
            {
                return false;
            }

            return true;
        }

        public static bool AddOrders(List<OrderModel> list)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(Config._connectionString))
                {
                    Connection.Open();
                    foreach (var order in list)
                    {
                        string addOrderQuery = $"INSERT INTO Zamowienie VALUES({order.ID_zamowienia},{order.ID_kartonu}, {(order.Pozycje.Any(x=>x.Czy_na_stanie == true)? 1: 0)},NULL)";
                        SqlCommand addOrderCommand = new SqlCommand(addOrderQuery, Connection);
                        addOrderCommand.ExecuteNonQuery();

                        foreach (var pozycja in order.Pozycje)
                        {
                            string addPozycjaQuery = $"INSERT INTO Pozycja VALUES('{pozycja.SKU}',{pozycja.Ilosc},{order.ID_zamowienia},{(pozycja.Czy_na_stanie ? 1 : 0)})";
                            SqlCommand addPozycjaCommand = new SqlCommand(addPozycjaQuery, Connection);
                            addPozycjaCommand.ExecuteNonQuery();
                        }
                    }

                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        public static bool UpdateCarton(UpdateCartonOrderPostModel form)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(Config._connectionString))
                {
                    //Zapytanie SQL
                    string query = $"UPDATE Zamowienie SET ID_kartonu = {form.id_kartonu} WHERE ID_zamowienia = {form.id_zamowienia}; ";

                    SqlCommand command = new SqlCommand(query, Connection);

                    Connection.Open();

                    command.ExecuteNonQuery();

                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

    }
}