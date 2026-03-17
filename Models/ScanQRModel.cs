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

                    var command = new MySqlCommand(@"insert into movements(id_product, count, id_user, id_type) values (@id, @count, @idUser, @idType)", connection);

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
