using Microsoft.Win32;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.WpfExtensions;
using QRcodeStorage.Services;
using QRcodeStorage.Views;
using QRcodeStorage.Views.UserControls;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using ZXing.Windows.Compatibility;


namespace QRcodeStorage.Pages
{
    public partial class ScanQR : Page
    {
        private BarcodeReader barcodeReader;
        private Loader loader = new();

        public ScanQR()
        {
            InitializeComponent();
            rbCamera.IsChecked = true;
        }
        private void rbCamera_Click(object sender, RoutedEventArgs e)
        {
            _ucFileInfo.Visibility = Visibility.Collapsed;
            ucCamera.Visibility = Visibility.Visible;
        }
        private void rbFile_Click(object sender, RoutedEventArgs e)
        {
            ucCamera.Visibility = Visibility.Collapsed;

            OpenFileDialog openFileDialog = new()
            {
                Filter = "Изображения|*.png;*.jpg;*.jpeg;*.bmp;*",
                Title = "Выберите изображение с QR-кодом"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    using (var bitmap = new Bitmap(openFileDialog.FileName))
                    {
                        if (barcodeReader == null)
                            barcodeReader = new BarcodeReader();

                        var result = barcodeReader.Decode(bitmap);

                        if (result != null)
                        {
                            (bool qrExist, DataView dataView) = loader.CheckAndLoadProduct(result.ToString());

                            if (qrExist)
                            {
                                _ucFileInfo.IsSuccess = true;
                                _ucFileInfo.Info = "Найден QR-код";
                                _ucFileInfo.QrInfo = result.Text;
                                _ucFileInfo.FilePath = openFileDialog.FileName;
                                _ucFileInfo.FileSize = new FileInfo(openFileDialog.FileName).Length;

                                Notification.Show(true, "Найден QR-код", $"Обнаружен '{result.Text}'");

                                grTip.Visibility = Visibility.Collapsed;
                                _ucProductInfo.Visibility = Visibility.Visible;
                                _ucProductInfo.Id = Convert.ToInt32(dataView[0]["id_product"]);
                                _ucProductInfo.ProductName = dataView[0]["name"].ToString();
                                _ucProductInfo.Category = dataView[0]["category"].ToString();
                                _ucProductInfo.Maker = dataView[0]["maker"].ToString();
                                _ucProductInfo.Count = Convert.ToInt32(dataView[0]["count"]);
                                _ucProductInfo.Place = dataView[0]["place"].ToString();
                                _ucProductInfo.Description = dataView[0]["description"].ToString();
                            }
                            else
                            {
                                _ucProductInfo.Visibility = Visibility.Collapsed;
                                _ucFileInfo.IsSuccess = false;
                                _ucFileInfo.Info = "QR не распознан";
                                _ucFileInfo.QrInfo = result.Text;
                                _ucFileInfo.FilePath = openFileDialog.FileName;
                                _ucFileInfo.FileSize = new FileInfo(openFileDialog.FileName).Length;
                                Notification.Show(false, "QR не распознан", $"'{result.Text}' Не существует");
                            }
                        }
                        else
                        {
                            _ucProductInfo.Visibility = Visibility.Collapsed;
                            _ucFileInfo.IsSuccess = false;
                            _ucFileInfo.Info = "QR-код не найден";
                            _ucFileInfo.QrInfo = "-";
                            _ucFileInfo.FilePath = openFileDialog.FileName;
                            _ucFileInfo.FileSize = new FileInfo(openFileDialog.FileName).Length;
                            Notification.Show(false, "Ошибка", "QR-код не найден на изображении");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _ucProductInfo.Visibility = Visibility.Collapsed;
                    _ucFileInfo.IsSuccess = false;
                    _ucFileInfo.Info = "Не удалось открыть файл";
                    _ucFileInfo.QrInfo = "-";
                    _ucFileInfo.FilePath = openFileDialog.FileName;
                    _ucFileInfo.FileSize = 0;
                    Notification.Show(false, "Ошибка", $"Не удалось открыть файл: {ex.Message}");
                }

                _ucFileInfo.Visibility = Visibility.Visible;
            }

        }
    }
}

