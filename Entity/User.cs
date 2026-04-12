using MySql.Data.MySqlClient;
using System.Windows;
using QRcodeStorage.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRcodeStorage.Views;

namespace QRcodeStorage.Entity
{
    internal class User : DataBase
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string MidName { get; set; }
        public string? LastName { get; set; }
        public int RoleId { get; set; }
        public User() { }
        public User(int id, string login, string firstName, string midName, string lastName, int roleId)
        {
            Id = id;
            Login = login;
            FirstName = firstName;
            MidName = midName;
            LastName = lastName;
            RoleId = roleId;
        }

        public bool Register(string login, string password, string firstName,
                                string midName, string lastName, int idRole = 2)
        {
            try
            {
                using (MySqlConnection connection = new(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO users(login, password, firstname, midname, lastname, id_role) " +
                        "VALUES (@login, @password, @firstName, @midName, @lastName, @idRole)";

                    using (MySqlCommand command = new(query, connection))
                    {
                        command.Parameters.AddWithValue("@login", login);
                        command.Parameters.AddWithValue("@password", BCrypt.Net.BCrypt.HashPassword(password));
                        command.Parameters.AddWithValue("@firstName", firstName);
                        command.Parameters.AddWithValue("@midName", midName);
                        command.Parameters.AddWithValue("@lastName", lastName);
                        command.Parameters.AddWithValue("@idRole", idRole);

                        Notification.Show(false, "Успех", $"Пользователь {login} создан");
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Notification.Show(false, "Ошибка", ex.Message);
                return false;
            }
        }

        public bool LoginUser(string login, string password)
        {
            try
            {
                using (MySqlConnection connection = new(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM users WHERE login = @login LIMIT 1";

                    using (MySqlCommand command = new(query, connection))
                    {
                        command.Parameters.AddWithValue("@login", login);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (BCrypt.Net.BCrypt.Verify(password, reader.GetString("password")))
                                {
                                    Id = reader.GetInt32("id_user");
                                    Login = reader.GetString("login");
                                    FirstName = reader.GetString("firstname");
                                    MidName = reader.GetString("midname");
                                    LastName = reader.IsDBNull(reader.GetOrdinal("lastname"))
                                        ? null
                                        : reader.GetString("lastname"); 
                                    RoleId = reader.GetInt32("id_role");

                                    Notification.Show(true, "Успех", $"Выполнен вход от {Login}");
                                    return true;
                                }
                            }

                            Notification.Show(false, "Ошибка", "Неправильный логин или пароль");
                            return false;
                        }
                    }
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
