using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.WpfExtensions;
using QRcodeStorage.Services;
using QRcodeStorage.Views;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;
using System.Windows.Threading;
using ZXing.Windows.Compatibility;
using Microsoft.Win32;
using QRcodeStorage.Views.UserControls;

namespace QRcodeStorage.Pages
{
    public partial class ScanQR : Page
    {
        public ScanQR()
        {
            InitializeComponent();

            rbCamera.IsChecked = true;

            this.Loaded += ScanQR_Loaded;
            this.Unloaded += ScanQR_Unloaded;
        }

        private async void ScanQR_Loaded(object sender, RoutedEventArgs e)
        {
            //await InitializeCameraAsync();
        }

        private async void ScanQR_Unloaded(object sender, RoutedEventArgs e)
        {
            //await StopCameraAsync();
        }

        

        //private void rbFile_Checked(object sender, RoutedEventArgs e)
        //{

        //    brCameraTip.Visibility = Visibility.Collapsed;
        //    brLoadingOverlay.Visibility = Visibility.Collapsed;

        //    OpenFileDialog openFileDialog = new()
        //    {
        //        Filter = "Изображения|*.png;*.jpg;*.jpeg;*.bmp;*",
        //        Title = "Выберите изображение с QR-кодом"
        //    };

        //    if (openFileDialog.ShowDialog() == true)
        //    {
        //        try
        //        {
        //            using (var bitmap = new Bitmap(openFileDialog.FileName))
        //            {
        //                if (barcodeReader == null)
        //                    barcodeReader = new BarcodeReader();
                        
        //                var result = barcodeReader.Decode(bitmap);

        //                if (result != null)
        //                {
        //                    (bool qrExist, DataView dataView) = loader.CheckAndLoadProduct(result.ToString());

        //                    if (qrExist)
        //                    {
        //                        Notification.Show(true, "Найден QR-код", $"Обнаружен '{result.Text}'");
        //                    }
        //                    else
        //                    {
        //                        Console.Beep(250, 300);
        //                        Notification.Show(false, "QR не распознан", $"'{result.Text}' Не существует");
        //                    }
        //                }
        //                else
        //                {
        //                    Console.Beep(250, 300);
        //                    Notification.Show(false, "Ошибка", "QR-код не найден на изображении");
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Notification.Show(false, "Ошибка", $"Не удалось открыть файл: {ex.Message}");
        //        }
        //    }
        //}
        //private void rbCamera_Checked(object sender, RoutedEventArgs e)
        //{
        //    InitializeCameraAsync();
        //}
    }
}
