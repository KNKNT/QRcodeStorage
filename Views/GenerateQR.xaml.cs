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

namespace QRcodeStorage.Pages
{
    /// <summary>
    /// Логика взаимодействия для GenerateQR.xaml
    /// </summary>
    public partial class GenerateQR : Page
    {
        Loader loader = new();
        Product product;
        DataView dataView;

        private void StackPanel_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) => cbShowQr.IsChecked = !cbShowQr.IsChecked;
        private void cbShowQr_Checked(object sender, RoutedEventArgs e) => dataView.RowFilter = "[Qr] = 0";
        private void cbShowQr_Unchecked(object sender, RoutedEventArgs e) => dataView.RowFilter = null;

        public GenerateQR()
        {
            InitializeComponent();
            dataView = loader.LoadDataTable("SELECT id_product, name, qr FROM ShowProducts");
            dgProducts.ItemsSource = dataView;
        }


        private void dgProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)dgProducts.SelectedItem;

            product = new()
            {
                Id = Convert.ToInt32(selectedRow[0]),
                Name = selectedRow[1].ToString()
            };

            tblProduct.Text = product.Name;

            string qrCode = $"{product.Id} | {product.Name}";
        }
    }
}
