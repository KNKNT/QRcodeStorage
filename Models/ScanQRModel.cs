using MySql.Data.MySqlClient;
using QRcodeStorage.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRcodeStorage.Models
{
    internal class ScanQRModel : DataBase
    {
        public bool MovementProduct(int id, int count, int idUser, int idType)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO movements(id_product, count, id_user, id_type) VALUES (@id, @count, @idUser, @idType);" +
                                   "UPDATE products SET count = count - @count WHERE id_product = @id;";

                    var command = new MySqlCommand(query, connection);

                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@count", count);
                    command.Parameters.AddWithValue("@idUser", idUser);
                    command.Parameters.AddWithValue("@idType", idType);

                    int result = command.ExecuteNonQuery();
                    Notification.Show(true, "Успех", $"Данные обновлены");
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Notification.Show(false, "Ошибка", ex.Message);
                return false;
            }
        }
    }
}
