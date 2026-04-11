using MySql.Data.MySqlClient;
using QRcodeStorage.Views;
using QRcodeStorage.Services;
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
                                                (@Name,@Count,@IdCategory,@Place,@IdMaker,@Description);
                                                SELECT last_insert_id();", connection);

                    command.Parameters.AddWithValue("@Name", product.Name);
                    command.Parameters.AddWithValue("@Count", product.Count);
                    command.Parameters.AddWithValue("@IdCategory", product.IdCategory ?? null);
                    command.Parameters.AddWithValue("@Place", product.Place ?? null);
                    command.Parameters.AddWithValue("@IdMaker", product.IdMaker ?? null);
                    command.Parameters.AddWithValue("@Description", product.Description ?? null);

                    int newId = Convert.ToInt32(command.ExecuteScalar());

                    command.CommandText = @"INSERT INTO movements(id_product, count, id_user, id_type)
                            VALUES (@id_product, @count, @id_user, @id_type);";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@id_product", newId);
                    command.Parameters.AddWithValue("@count", product.Count);
                    command.Parameters.AddWithValue("@id_user", Session.CurrentUser.Id);
                    command.Parameters.AddWithValue("@id_type", 2);

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
