using Microsoft.Data.SqlClient;
using WarehouseManagmentAPI.Database.DatabaseModels;

namespace WarehouseManagmentAPI.Database.DatabaseControllers
{
    public static class ConfigurationDbC
    {

        //pobieranie danych z bazy do modelu ConfigurationModel
        public static ConfigurationModel GetConfiguration(string user)
        {
            ConfigurationModel configuration = new ConfigurationModel();

            using (SqlConnection Connection = new SqlConnection(Config._connectionString))
            {
                //Zapytanie SQL
                SqlCommand command = new SqlCommand($"SELECT * FROM Konfiguracja WHERE Nazwa_uzytkownika = {user}", Connection);
                Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                //Odczyt wierszy z SQL
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