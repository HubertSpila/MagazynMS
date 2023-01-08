using Microsoft.Data.SqlClient;
using WarehouseManagmentAPI.Database.DatabaseModels;

namespace WarehouseManagmentAPI.Database.DatabaseControllers
{
    public static class CartonDbC
    {
        private static string _connectionString = @"Data Source=.\SQLExpress;Initial Catalog=MagazynMS;Trusted_Connection=True";
        public static List<CartonModel> GetCartons()
        {
            List<CartonModel> cartons = new List<CartonModel>();

            using (SqlConnection Connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand($"SELECT * FROM Karton", Connection);
                Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

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
    }
}
