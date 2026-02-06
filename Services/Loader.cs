using MySql.Data.MySqlClient;
using QRcodeStorage.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRcodeStorage.Services
{
    class Loader : DataBase
    {
        DataTable dataTable = new();
        public DataView LoadDataTable(string query)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var command = new MySqlCommand(query, connection);

                using (var reader = command.ExecuteReader())
                    dataTable.Load(reader);
            }
            return dataTable.DefaultView;
        }
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
    }
}
