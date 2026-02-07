using QRcodeStorage.Models;
using QRcodeStorage.Services;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;
using ZXing.Windows.Compatibility;


namespace QRcodeStorage.Pages
{
    /// <summary>
    /// Логика взаимодействия для GenerateQR.xaml
    /// </summary>
    public partial class GenerateQR : Page
    {
        Loader loader = new();
        DataView dataView;
        int size = 100;
        int columns = 5;

        private void cbShowQr_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) => cbShowQr.IsChecked = !cbShowQr.IsChecked;
        private void cbShowQr_Checked(object sender, RoutedEventArgs e) => dataView.RowFilter = "[Qr] = '0'";
        private void cbShowQr_Unchecked(object sender, RoutedEventArgs e) => dataView.RowFilter = null;
        private void dgProducts_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateQr();

        public GenerateQR()
        {
            InitializeComponent();
            dataView = loader.LoadDataTable("SELECT id_product, name, maker, qr FROM ShowProducts");
            dgProducts.ItemsSource = dataView;
        }

        private void UpdateQr()
        {
            qrUniformGrid.Children.Clear();

            foreach (var selectedItem in dgProducts.SelectedItems)
            {
                if (selectedItem is DataRowView selectedRow)
                {
                    var product = new Product()
                    {
                        Id = Convert.ToInt32(selectedRow[0]),
                        Name = selectedRow[1].ToString(),
                    };

                    var stackPanel = new StackPanel()
                    {
                        Margin = new Thickness(10),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Top
                    };

                    var border = new Border();
                    

                    string qrCode = $"{product.Id} | {product.Name}";
                    var qrImage = GenerateQRCode(qrCode);

                    var image = new System.Windows.Controls.Image()
                    {
                        Source = qrImage,
                        Stretch = Stretch.UniformToFill,
                        Width = size,
                        Height = size
                    };

                    var textBlock = new TextBlock()
                    {
                        Text = product.Name,
                        TextAlignment = TextAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        MaxWidth = 160,
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(0, 1, 0, 0),
                        FontWeight = FontWeights.Medium
                    };

                    border.Child = image;
                    stackPanel.Children.Add(border);
                    stackPanel.Children.Add(textBlock);

                    qrUniformGrid.Children.Add(stackPanel);
                }
            }
        }

        private void cbSize_DropDownClosed(object sender, EventArgs e)
        {
            (size, columns) = cbSize.SelectedIndex switch
            {
                0 => (100, 5),
                1 => (180, 3),
                2 => (270, 2), 
                _ => (100, 5)
            };
            qrUniformGrid.Columns = columns;
            UpdateQr();
        }
        private WriteableBitmap GenerateQRCode(string text)
        {
            var barcodeWriter = new BarcodeWriter<WriteableBitmap>
            {
                Format = BarcodeFormat.QR_CODE,
                Renderer = new WriteableBitmapRenderer(),

                Options = new QrCodeEncodingOptions
                {
                    Width = 300,
                    Height = 300,
                    CharacterSet = "UTF-8",
                    ErrorCorrection = ErrorCorrectionLevel.H
                },
            };

            return barcodeWriter.Write(text);
        }
    }
}
