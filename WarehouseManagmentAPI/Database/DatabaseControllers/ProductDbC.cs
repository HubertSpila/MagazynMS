using Microsoft.Data.SqlClient;
using WarehouseManagmentAPI.Controllers.PostModels;
using WarehouseManagmentAPI.Database.DatabaseModels;

namespace WarehouseManagmentAPI.Database.DatabaseControllers
{
    public static class ProductDbC
    {
        //pobieranie danych z bazy do modelu ProductModel (Lista)
        public static List<ProductModel> GetProducts()
        {
            List<ProductModel> products = new List<ProductModel>();
            List<CartonModel>? cartons = CartonDbC.GetCartons();

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
                        Karton = cartons.Where(x=>x.ID_kartonu == (int)reader[2]).FirstOrDefault(),
                        Stan_magazynowy = (int)reader[3],
                        Potrzebna_ilosc = (int)reader[4]
                    });
                }

                reader.Close();
                Connection.Close();
            }

            return products;
        }

        //pobieranie danych z bazy do modelu ProductModel
        public static ProductModel GetProduct(string sku)
        {
            ProductModel product = new ProductModel();

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
                        Potrzebna_ilosc = (int)reader[4]
                    };
                }

                reader.Close();
                Connection.Close();
            }

            return product;
        }

        //pobieranie danych z bazy do modelu ProductModel (Lista) pobiera tylko dostępne produkty
        public static List<ProductModel> GetAvailableProducts()
        {
            List<ProductModel> products = new List<ProductModel>();
            List<CartonModel>? cartons = CartonDbC.GetCartons();

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
                        Potrzebna_ilosc = (int)reader[4]
                    });
                }

                reader.Close();
                Connection.Close();
            }

            return products;
        }
        public static void AddProduct(AddProductPostModel form)
        {
            using (SqlConnection Connection = new SqlConnection(Config._connectionString))
            {
                //Zapytanie SQL
                SqlCommand command = new SqlCommand($"INSERT INTO Produkt VALUES ('{form.SKU}', '{form.Nazwa_produktu}', {form.ID_kartonu}, {form.Stan_magazynowy}, {form.Potrzebna_ilosc}); ", Connection);

                Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                reader.Close();
                Connection.Close();
            }

        }
    }
}
