﻿using Microsoft.Data.SqlClient;
using WarehouseManagmentAPI.Database.DatabaseModels;

namespace WarehouseManagmentAPI.Database.DatabaseControllers
{
    public static class ProductDbC
    {
        public static List<ProductModel> GetProducts()
        {
            List<ProductModel> products = new List<ProductModel>();

            using (SqlConnection Connection = new SqlConnection(Config._connectionString))
            {
                SqlCommand command = new SqlCommand($"SELECT * FROM Towar", Connection);
                Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    products.Add(new ProductModel()
                    {
                        SKU = reader[0].ToString(),
                        Nazwa_produktu = reader[1].ToString(),
                        ID_kartonu = (int)reader[2],
                        Stan_magazynowy = (int)reader[3],
                        Potrzebna_ilosc = (int)reader[4]
                    });
                }

                reader.Close();
                Connection.Close();
            }

            return products;
        }
        public static ProductModel GetProduct(string sku)
        {
            ProductModel product = new ProductModel();

            using (SqlConnection Connection = new SqlConnection(Config._connectionString))
            {
                SqlCommand command = new SqlCommand($"SELECT * FROM Towar WHERE SKU = {sku}", Connection);
                Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    product = new ProductModel()
                    {
                        SKU = reader[0].ToString(),
                        Nazwa_produktu = reader[1].ToString(),
                        ID_kartonu = (int)reader[2],
                        Stan_magazynowy = (int)reader[3],
                        Potrzebna_ilosc = (int)reader[4]
                    };
                }

                reader.Close();
                Connection.Close();
            }

            return product;
        }
    }
}