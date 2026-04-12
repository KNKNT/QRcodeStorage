using QRcodeStorage.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QRcodeStorage.Views
{
    /// <summary>
    /// Логика взаимодействия для OtherPage.xaml
    /// </summary>
    public partial class OtherPage : Page
    {
        DataView dataView;
        Loader loader = new();
        string query;
        public OtherPage()
        {
            InitializeComponent();
            
            rbUsers.IsChecked = true;
        }

        private void ClearData_Click(object sender, RoutedEventArgs e)
        {
            dataView.Table.RejectChanges();
        }
        private void HashPasswordsInDataTable()
        {
            if (dataView == null || dataView.Table == null) return;

            var table = dataView.Table;

            foreach (DataRow row in table.Rows)
            {
                if (row.RowState == DataRowState.Added || row.RowState == DataRowState.Modified)
                {
                    string currentPassword = row["Пароль", DataRowVersion.Current].ToString();

                    if (row.RowState == DataRowState.Modified)
                    {
                        string originalPassword = row["Пароль", DataRowVersion.Original].ToString();

                        if (currentPassword == originalPassword)
                            continue;
                    }

                    if (!string.IsNullOrWhiteSpace(currentPassword))
                    {
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(currentPassword);
                        row["Пароль"] = hashedPassword;
                    }
                }
            }
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (rbUsers.IsChecked == true)
                HashPasswordsInDataTable();

            loader.SaveChanges(dataView, query);
        }
        private void rbUsers_Checked(object sender, RoutedEventArgs e)
        {
            query = "SELECT id_user as 'Id', login as 'Логин', password as 'Пароль',  firstname as 'Фамилия', midname as 'Имя', lastname as 'Отчество', id_role as 'Id Роли' FROM qrstorage.users;";
            dataView = loader.LoadDataTable(query);
            dgProducts.ItemsSource = dataView;
        }

        private void rbRoles_Checked(object sender, RoutedEventArgs e)
        {
            query = "SELECT id_role as 'Id Роли', role as 'Администратор' FROM qrstorage.roles;";
            dataView = loader.LoadDataTable(query);
            dgProducts.ItemsSource = dataView;
        }

        private void rbProducts_Checked(object sender, RoutedEventArgs e)
        {
            query = "SELECT id_product as 'Id Товара', name as 'Товар', count as 'Количество', id_category as 'Id Категории', place  as 'Место',id_maker as 'Id Производителя', description as 'Описание', Qr FROM qrstorage.products;";
            dataView = loader.LoadDataTable(query);
            dgProducts.ItemsSource = dataView;
        }

        private void rbCategories_Checked(object sender, RoutedEventArgs e)
        {
            query = "SELECT id_category as 'Id Категории', category as 'Категория' FROM qrstorage.categories;";
            dataView = loader.LoadDataTable(query);
            dgProducts.ItemsSource = dataView;
        }

        private void rbMakers_Checked(object sender, RoutedEventArgs e)
        {
            query = "SELECT id_maker as 'Id Производителя', maker as 'Производитель' FROM qrstorage.makers;";
            dataView = loader.LoadDataTable(query);
            dgProducts.ItemsSource = dataView;
        }

        private void rbMovements_Checked(object sender, RoutedEventArgs e)
        {
            query = "SELECT id_movement as 'Id Движения', id_product as 'Id Товара', count as 'Количество', id_user as 'Id Пользователя', date as 'Дата', id_type as 'Id Типа' FROM qrstorage.movements;";
            dataView = loader.LoadDataTable(query);
            dgProducts.ItemsSource = dataView;
        }

        private void rbTypes_Checked(object sender, RoutedEventArgs e)
        {
            query = "SELECT id_type as 'Id Типа', type as 'Тип' FROM qrstorage.types;";
            dataView = loader.LoadDataTable(query);
            dgProducts.ItemsSource = dataView;
        }
    }
}
