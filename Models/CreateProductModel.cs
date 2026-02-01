using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QRcodeStorage.Models
{
    class CreateProductModel : DataBase
    {
        public List<Categories> LoadCategories()
        {
            var categories = new List<Categories>();

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var command = new MySqlCommand("SELECT * FROM categories", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new Categories
                        {
                            Id = reader.GetInt32("id_category"),
                            Category = reader.GetString("category")
                        });
                    }
                }
            }
            return categories;
        }

        public List<Makers> LoadMakers()
        {
            var makers = new List<Makers>();

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var command = new MySqlCommand("SELECT * FROM makers", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        makers.Add(new Makers
                        {
                            Id = reader.GetInt32("id_maker"),
                            Maker = reader.GetString("maker")
                        });
                    }
                }
            }
            return makers;
        }

        public bool InsertProduct(Product product)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    var command = new MySqlCommand(@"INSERT INTO products 
                                                (Name,Count,Id_Category,Place,Id_Maker,Description) 
                                                VALUES 
                                                (@Name,@Count,@IdCategory,@Place,@IdMaker,@Description)", connection);

                    command.Parameters.AddWithValue("@Name", product.Name);
                    command.Parameters.AddWithValue("@Count", product.Count);
                    command.Parameters.AddWithValue("@IdCategory", product.IdCategory ?? null);
                    command.Parameters.AddWithValue("@Place", product.Place ?? null);
                    command.Parameters.AddWithValue("@IdMaker", product.IdMaker ?? null);
                    command.Parameters.AddWithValue("@Description", product.Description ?? null);

                    int result = command.ExecuteNonQuery();
                    MessageBox.Show("Данные добавлены!");
                    return result > 0;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
}
