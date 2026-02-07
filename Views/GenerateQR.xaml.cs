using QRcodeStorage.Models;
using QRcodeStorage.Services;
using System.Data;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ZXing;
using ZXing.QrCode;
using ZXing.Rendering;
using ZXing.Windows.Compatibility;


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

        private void cbShowQr_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) => cbShowQr.IsChecked = !cbShowQr.IsChecked;
        private void cbShowQr_Checked(object sender, RoutedEventArgs e) => dataView.RowFilter = "[Qr] = '0'";
        private void cbShowQr_Unchecked(object sender, RoutedEventArgs e) => dataView.RowFilter = null;

        public GenerateQR()
        {
            InitializeComponent();
            dataView = loader.LoadDataTable("SELECT id_product, name, maker, qr FROM ShowProducts");
            dgProducts.ItemsSource = dataView;
        }


        private void dgProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)dgProducts.SelectedItem;
            product = new()
            {
                Id = Convert.ToInt32(selectedRow[0]),
                Name = selectedRow[1].ToString(),
            };

            tblProduct.Text = $"{product.Name} {selectedRow[2].ToString()}";

            string qrCode = $"{product.Id} | {product.Name}";
            var barcodeWriter = new BarcodeWriter<WriteableBitmap>
            {
                Format = BarcodeFormat.QR_CODE,
                Renderer = new WriteableBitmapRenderer(),
                Options = new QrCodeEncodingOptions
                {
                    Width = 150,
                    Height = 150,
                    Margin = 1
                },
            };

            var barcodeBitmap = barcodeWriter.Write(qrCode);
            imgQr.Source = barcodeBitmap;
        }
    }
}
