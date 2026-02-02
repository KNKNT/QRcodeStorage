using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRcodeStorage.Models
{
    class ShowProductModel : CreateProductModel
    {
        DataTable dataTable = new();
        public DataView LoadProduct()
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var command = new MySqlCommand("SELECT * FROM ShowProducts", connection);

                using (var reader = command.ExecuteReader())
                    dataTable.Load(reader);
                
            }
            return dataTable.DefaultView;
        }
    }
}
