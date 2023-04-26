using Microsoft.Data.SqlClient;
using WarehouseManagmentAPI.Controllers.PostModels;
using WarehouseManagmentAPI.Database.DatabaseModels;
using WarehouseManagmentAPI.Tools;

namespace WarehouseManagmentAPI.Database.DatabaseControllers
{
    public static class ProductDbC
    {
        //pobieranie danych z bazy do modelu ProductModel (Lista)
        public static List<ProductModel> GetProducts()
        {
            List<ProductModel> products = new List<ProductModel>();
            List<CartonModel>? cartons = CartonDbC.GetCartons();
            try
            {
                using (SqlConnection Connection = new SqlConnection(Config._connectionString))
                {
                    SqlCommand command = new SqlCommand($"SELECT * FROM Produkt", Connection);
                    Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    //Odczyt wierszy z SQL
                    while (reader.Read())
                    {
                        products.Add(new ProductModel()
                        {
                            SKU = reader[0].ToString(),
                            Nazwa_produktu = reader[1].ToString(),
                            Karton = cartons.Where(x => x.ID_kartonu == (int)reader[2]).FirstOrDefault(),
                            Stan_magazynowy = (int)reader[3],
                            Potrzebna_ilosc = (int)reader[4],
                            Parametry = Convert.IsDBNull(reader[5]) ? "" : reader[5].ToString()
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

            return products;
        }

        //pobieranie danych z bazy do modelu ProductModel
        public static ProductModel GetProduct(string sku)
        {
            ProductModel product = new ProductModel();
            try
            {
                using (SqlConnection Connection = new SqlConnection(Config._connectionString))
                {
                    //Zapytanie SQL
                    SqlCommand command = new SqlCommand($"SELECT * FROM Produkt WHERE SKU = '{sku}'", Connection);

                    Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    //Odczyt wierszy z SQL
                    while (reader.Read())
                    {
                        product = new ProductModel()
                        {
                            SKU = reader[0].ToString(),
                            Nazwa_produktu = reader[1].ToString(),
                            Karton = CartonDbC.GetCarton((int)reader[2]),
                            Stan_magazynowy = (int)reader[3],
                            Potrzebna_ilosc = (int)reader[4],
                            Parametry = Convert.IsDBNull(reader[5]) ? "" : reader[5].ToString()

                        };
                    }

                    reader.Close();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                NLogConfig.WriteLog(ex.ToString());
            }

            return product;
        }

        //pobieranie danych z bazy do modelu ProductModel (Lista) pobiera tylko dostępne produkty
        public static List<ProductModel> GetAvailableProducts()
        {
            List<ProductModel> products = new List<ProductModel>();
            List<CartonModel>? cartons = CartonDbC.GetCartons();
            try
            {

                using (SqlConnection Connection = new SqlConnection(Config._connectionString))
                {
                    //Zapytanie SQL
                    SqlCommand command = new SqlCommand($"SELECT * FROM Produkt WHERE Stan_magazynowy > 0", Connection);

                    Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    //Odczyt wierszy z SQL
                    while (reader.Read())
                    {
                        products.Add(new ProductModel()
                        {
                            SKU = reader[0].ToString(),
                            Nazwa_produktu = reader[1].ToString(),
                            Karton = cartons.Where(x => x.ID_kartonu == (int)reader[2]).FirstOrDefault(),
                            Stan_magazynowy = (int)reader[3],
                            Potrzebna_ilosc = (int)reader[4],
                            Parametry = Convert.IsDBNull(reader[5]) ? "" : reader[5].ToString()

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

            return products;
        }
        public static void AddProduct(AddProductPostModel form)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(Config._connectionString))
                {
                    //Zapytanie SQL
                    SqlCommand command = new SqlCommand($"INSERT INTO Produkt VALUES ('{form.SKU}', '{form.Nazwa_produktu}', {form.ID_kartonu}, {form.Stan_magazynowy}, {form.Potrzebna_ilosc}, '{form.Parametry}'); ", Connection);

                    Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    reader.Close();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                NLogConfig.WriteLog(ex.ToString());
            }

        }
        public static void UpdateProduct(ChangeProductQuantityPostModel form)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(Config._connectionString))
                {
                    //Zapytanie SQL
                    SqlCommand command = new SqlCommand($"UPDATE Produkt SET Stan_magazynowy = {form.ilosc} WHERE SKU = '{form.sku}' AND Parametry = '{form.parametry}' ; ", Connection);

                    Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Close();

                    if (form.parametry == string.Empty)
                    {
                        SqlCommand command2 = new SqlCommand($"UPDATE Produkt SET Stan_magazynowy = {form.ilosc} WHERE SKU = '{form.sku}' AND Parametry is NULL ; ", Connection);
                        SqlDataReader reader2 = command2.ExecuteReader();
                        reader2.Close();
                    }

                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                NLogConfig.WriteLog(ex.ToString());
            }

        }
        public static void WarehousOut(WarehouseOutPostModel form)
        {
            try
            {
                ProductModel prod = GetProducts().Where(x=>x.SKU == form.SKU && x.Parametry == form.Parametry).FirstOrDefault();

                using (SqlConnection Connection = new SqlConnection(Config._connectionString))
                {
                    //Zapytanie SQL
                    SqlCommand command = new SqlCommand($"UPDATE Produkt SET Stan_magazynowy = {--prod.Stan_magazynowy} WHERE SKU = '{form.SKU}' AND Parametry = '{form.Parametry}' ; ", Connection);

                    Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Close();

                    if (form.Parametry == string.Empty)
                    {
                        SqlCommand command2 = new SqlCommand($"UPDATE Produkt SET Stan_magazynowy = {prod.Stan_magazynowy} WHERE SKU = '{form.SKU}' AND Parametry is NULL ; ", Connection);
                        SqlDataReader reader2 = command2.ExecuteReader();
                        reader2.Close();
                    }

                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                NLogConfig.WriteLog(ex.ToString());
            }

        }
    }
}
