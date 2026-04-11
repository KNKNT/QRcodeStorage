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
        DataView dataView = new();
        Loader loader = new();
        public OtherPage()
        {
            InitializeComponent();
            
            rbUsers.IsChecked = true;
        }

        private void ClearData_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rbUsers_Checked(object sender, RoutedEventArgs e)
        {
            dataView = loader.LoadDataTable("SELECT * FROM Users");
            dgProducts.ItemsSource = dataView;
        }

        private void rbRoles_Checked(object sender, RoutedEventArgs e)
        {
            dataView = loader.LoadDataTable("SELECT * FROM Roles");
            dgProducts.ItemsSource = dataView;
        }

        private void rbProducts_Checked(object sender, RoutedEventArgs e)
        {
            dataView = loader.LoadDataTable("SELECT * FROM Products");
            dgProducts.ItemsSource = dataView;
        }

        private void rbCategories_Checked(object sender, RoutedEventArgs e)
        {
            dataView = loader.LoadDataTable("SELECT * FROM Categories");
            dgProducts.ItemsSource = dataView;
        }

        private void rbMakers_Checked(object sender, RoutedEventArgs e)
        {
            dataView = loader.LoadDataTable("SELECT * FROM Makers");
            dgProducts.ItemsSource = dataView;
        }

        private void rbMovements_Checked(object sender, RoutedEventArgs e)
        {
            dataView = loader.LoadDataTable("SELECT * FROM Movements");
            dgProducts.ItemsSource = dataView;
        }

        private void rbTypes_Checked(object sender, RoutedEventArgs e)
        {
            dataView = loader.LoadDataTable("SELECT * FROM Types");
            dgProducts.ItemsSource = dataView;
        }
    }
}
