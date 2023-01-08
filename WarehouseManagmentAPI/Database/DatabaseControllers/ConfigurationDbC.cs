﻿using Microsoft.Data.SqlClient;
using WarehouseManagmentAPI.Database.DatabaseModels;

namespace WarehouseManagmentAPI.Database.DatabaseControllers
{
    public static class ConfigurationDbC
    {
        public static ConfigurationModel GetConfiguration(string user)
        {
            ConfigurationModel configuration = new ConfigurationModel();

            using (SqlConnection Connection = new SqlConnection(Config._connectionString))
            {
                SqlCommand command = new SqlCommand($"SELECT * FROM Konfiguracja", Connection);
                Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    configuration.BaseLinker_token = reader[0].ToString();
                    configuration.Nazwa_uzytkownika = reader[1].ToString();
                }

                reader.Close();
                Connection.Close();
            }

            return configuration;
        }
    }
}