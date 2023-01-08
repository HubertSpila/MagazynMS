﻿using Microsoft.Data.SqlClient;
using WarehouseManagmentAPI.Database.DatabaseModels;

namespace WarehouseManagmentAPI.Database.DatabaseControllers
{
    public static class StatisticsDbC
    {
        public static List<StatisticModel> GetStatistics()
        {
            List<StatisticModel> statistics = new List<StatisticModel>();

            using (SqlConnection Connection = new SqlConnection(Config._connectionString))
            {
                SqlCommand command = new SqlCommand($"SELECT * FROM Statystyki", Connection);
                Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    statistics.Add(new StatisticModel()
                    {
                        Data_sprzedazy = (DateTime)reader[0],
                        Ilosc = (int)reader[1],
                        SKU = reader[2].ToString()
                    });
                }

                reader.Close();
                Connection.Close();
            }

            return statistics;
        }
    }
}
