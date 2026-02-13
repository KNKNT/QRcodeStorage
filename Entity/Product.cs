using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRcodeStorage.Models
{
    class Product : DataBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public int? IdCategory { get; set; }
        public string? Place { get; set; }
        public int? IdMaker { get; set; }
        public string? Description { get; set; }

        public void UpdateStatus(List<int> id)
        {
            string idList = string.Join(",", id);

            string query = $@"UPDATE Products 
                            SET Qr = 1
                            WHERE Qr = 0
                            AND id_product IN ({idList})";

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
        }
    }
}
