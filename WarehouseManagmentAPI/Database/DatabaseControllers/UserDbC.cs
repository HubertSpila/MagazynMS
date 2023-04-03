using Microsoft.Data.SqlClient;
using WarehouseManagmentAPI.Controllers.PostModels;
using WarehouseManagmentAPI.Database.DatabaseModels;
using WarehouseManagmentAPI.Tools;

namespace WarehouseManagmentAPI.Database.DatabaseControllers
{
    public static class UserDbC
    {
        //pobieranie danych z bazy do modelu UserModel (Lista)
        public static List<UserModel> GetSUsers()
        {
            List<UserModel> users = new List<UserModel>();
            try
            {
                using (SqlConnection Connection = new SqlConnection(Config._connectionString))
                {
                    //Zapytanie SQL
                    SqlCommand command = new SqlCommand($"SELECT * FROM Uzytkownik", Connection);

                    Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    //Odczyt wierszy z SQL
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
            }
            catch (Exception ex)
            {
                NLogConfig.WriteLog(ex.ToString());
            }

            return users;
        }

        //Sprawdzanie poprawności danych do logowania
        public static bool IsOkUser(AuthenticationPostModel form)
        {
            //Sprawdzanie czy wszystkie pola są wypełnione danymi
            if (form == null || form.UserName == null || form.Password == null
               || form.UserName == string.Empty || form.Password == string.Empty) return false;

            bool result = false;

            try
            {
                using (SqlConnection Connection = new SqlConnection(Config._connectionString))
                {
                    //Zapytanie SQL
                    SqlCommand command = new SqlCommand($"SELECT * FROM Uzytkownik WHERE Nazwa_uzytkownika = '{form.UserName}' AND Haslo = '{form.Password}'", Connection);
                    Connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    //Odczyt wierszy z SQL
                    while (reader.Read())
                    {
                        if (reader[1].ToString() == form.Password)
                            result = true;
                    }

                    reader.Close();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                NLogConfig.WriteLog(ex.ToString());
            }

            return result;
        }
    }
}
