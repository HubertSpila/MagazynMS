using Microsoft.Data.SqlClient;
using WarehouseManagmentAPI.Database.DatabaseModels;

namespace WarehouseManagmentAPI.Database.DatabaseControllers
{
    public static class UserDbC
    {
        public static List<UserModel> GetSUsers()
        {
            List<UserModel> users = new List<UserModel>();

            using (SqlConnection Connection = new SqlConnection(Config._connectionString))
            {
                SqlCommand command = new SqlCommand($"SELECT * FROM Uzytkownik", Connection);
                Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(new UserModel()
                    {
                        Nazwa_uzytkownika = reader[0].ToString(),
                        Haslo = reader[1].ToString()
                    });
                }

                reader.Close();
                Connection.Close();
            }

            return users;
        }
    }
}
