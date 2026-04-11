using QRcodeStorage.Models;
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
    /// Логика взаимодействия для AddProductsPage.xaml
    /// </summary>
    public partial class AddProductsPage : Page
    {
        DataView dataView = new();
        Loader loader = new();

        private int count;
        


        public AddProductsPage()
        {
            InitializeComponent();
            LoadTable();
        }
        
        private void LoadTable()
        {
            dataView = loader.LoadDataTable("SELECT * FROM ShowProducts");
            dgProducts.ItemsSource = dataView;
        }

        private void dgProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView row = (DataRowView)dgProducts.SelectedItem;
            count = Convert.ToInt32(row["count"]);
            tblCount.Text = count.ToString();
            Counter.IsEnabled = true;
        }

        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            if(count > 1)
            {
                count--;
                tblCount.Text = count.ToString();
            }
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            count++;
            tblCount.Text = count.ToString();
        }

        private void tblCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(uint.TryParse(tblCount.Text, out uint newCount) & newCount != 0)
            {
                Counter.BorderBrush = Brushes.Gray;
                count = (int)newCount;
            }
            else
            {
                Notification.Show(false, "Ошибка", "Неверный формат");
                Counter.BorderBrush = Brushes.Red;
            }
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
