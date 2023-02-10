﻿using Microsoft.Data.SqlClient;
using System.Collections.Specialized;
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

    }
}
