using Microsoft.Data.SqlClient;
using WarehouseManagmentAPI.Controllers.PostModels;
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

        public static bool IsOkUser(AuthenticationPostModel form)
        {
            if (form == null || form.UserName == null || form.Password == null) return false;

            bool result = false;
            using (SqlConnection Connection = new SqlConnection(Config._connectionString))
            {
                SqlCommand command = new SqlCommand($"SELECT * FROM Uzytkownik WHERE Nazwa_uzytkownika = '{form.UserName}' AND Haslo = '{form.Password}'", Connection);
                Connection.Open();
                
                SqlDataReader reader = command.ExecuteReader();
                result = reader.HasRows;

                reader.Close();
                Connection.Close();
            }
            //do naprawy
            return result;
        }
    }
}
