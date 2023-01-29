﻿using Microsoft.Data.SqlClient;
using WarehouseManagmentAPI.Database.DatabaseModels;

namespace WarehouseManagmentAPI.Database.DatabaseControllers
{
    public static class CartonDbC
    {
        //pobieranie danych z bazy do modelu CartonModel (Lista)
        public static List<CartonModel> GetCartons()
        {
            List<CartonModel> cartons = new List<CartonModel>();

            using (SqlConnection Connection = new SqlConnection(Config._connectionString))
            {
                //Zapytanie SQL
                SqlCommand command = new SqlCommand($"SELECT * FROM Karton", Connection);
                
                Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                //Odczyt wierszy SQL
                while (reader.Read())
                {
                    cartons.Add(new CartonModel()
                    {
                        ID_kartonu = (int)reader[0],
                        Wysokosc = (int)reader[1],
                        Szerokosc = (int)reader[2],
                        Glebokosc = (int)reader[3],
                        Stan_magazynowy = (int)reader[4],
                        Potrzebna_ilosc = (int)reader[5],

                    });
                }

                reader.Close();
                Connection.Close();
            }

            return cartons;
        }


        //pobieranie danych z bazy do modelu CartonModel
        public static CartonModel GetCarton(string id)
        {
            CartonModel carton = new CartonModel();

            using (SqlConnection Connection = new SqlConnection(Config._connectionString))
            {
                //Zapytanie SQL
                SqlCommand command = new SqlCommand($"SELECT * FROM Karton WHERE ID_kartonu = {id}", Connection);
                
                Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                //Odczyt wierszySQL
                while (reader.Read())
                {
                    carton = new CartonModel()
                    {
                        ID_kartonu = (int)reader[0],
                        Wysokosc = (int)reader[1],
                        Szerokosc = (int)reader[2],
                        Glebokosc = (int)reader[3],
                        Stan_magazynowy = (int)reader[4],
                        Potrzebna_ilosc = (int)reader[5],

                    };
                }

                reader.Close();
                Connection.Close();
            }

            return carton;
        }
    }
}
