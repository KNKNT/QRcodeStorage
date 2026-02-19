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
        private Loader loader = new();
        private BarcodeReader barcodeReader;
        private VideoCapture capture;
        private DispatcherTimer timer;
        private bool isCameraReady = false;
        private bool isScanningEnabled = true;

        public ScanQR()
        {
            InitializeComponent();

            ShowLoading(true);
            rbCamera.IsChecked = true;

            this.Loaded += ScanQR_Loaded;
            this.Unloaded += ScanQR_Unloaded;
        }

        private async void ScanQR_Loaded(object sender, RoutedEventArgs e)
        {
            await InitializeCameraAsync();
        }

        private async void ScanQR_Unloaded(object sender, RoutedEventArgs e)
        {
            await StopCameraAsync();
        }

        private async Task InitializeCameraAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    capture = new VideoCapture(0);

                    if (!capture.IsOpened())
                    {
                        for (int i = 1; i < 5; i++)
                        {
                            capture = new VideoCapture(i);
                            if (capture.IsOpened())
                                break;
                        }
                    }

                    isCameraReady = capture != null && capture.IsOpened();
                }
                catch (Exception ex)
                {
                    Notification.Show(false, "Ой", $"Ошибка инициализации камеры: {ex.Message}");
                    isCameraReady = false;
                }
            });

            Dispatcher.Invoke(() =>
            {
                if (isCameraReady)
                {
                    barcodeReader = new BarcodeReader();

                    timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromMilliseconds(60);
                    timer.Tick += Timer_Tick;
                    timer.Start();

                    ShowLoading(false);
                    brCameraTip.Visibility = Visibility.Visible;

                }
                else
                {
                    LoadingText.Text = "Не удалось открыть камеру";
                    brCameraTip.Visibility = Visibility.Collapsed;
                    LoadingSpinner.Visibility = Visibility.Collapsed;
                }
            });
        }

        public Task StopCameraAsync()
        {
            return Task.Run(() =>
            {
                try
                {
                    Dispatcher.Invoke(() =>
                    {
                        timer?.Stop();
                        timer = null;
                    });

                    capture?.Release();
                    capture?.Dispose();
                    capture = null;
                }
                catch (Exception ex)
                {
                    Notification.Show(false, "Ой", $"Ошибка остановки камеры: {ex.Message}");
                }
            });
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (capture != null && capture.IsOpened())
            {
                using (Mat frame = new Mat())
                {
                    capture.Read(frame);
                    if (!frame.Empty())
                    {
                        imgCamera.Source = frame.ToBitmapSource();
                        if (isScanningEnabled)
                            ScanQRCode(frame);
                    }
                }
            }
        }

        private void ScanQRCode(Mat frame)
        {
            using (var bitmap = frame.ToBitmap())
            {
                var result = barcodeReader.Decode(bitmap);

                if (result != null)
                {
                    (bool qrExist, DataView dataView) = loader.CheckAndLoadProduct(result.ToString());

                    if (qrExist)
                    {
                        Console.Beep(820, 300);
                        Notification.Show(true, "Найден QR-код", $"Обнаружен '{result.Text}'");
                        Coldown();
                    }
                    else
                    {
                        Console.Beep(250, 300);
                        Notification.Show(false, "QR не распознан", $"'{result.Text}' Не существует");
                        Coldown();
                    }
                }
            }
        }

        private void Coldown(int seconds = 1)
        {
            isScanningEnabled = false;

            timer.Interval = TimeSpan.FromSeconds(seconds);
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                isScanningEnabled = true;
            };
            timer.Start();
        }

        private void ShowLoading(bool show)
        {
            brLoadingOverlay.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            grCamera.Visibility = show ? Visibility.Collapsed : Visibility.Visible;
        }

        private void rbFile_Checked(object sender, RoutedEventArgs e)
        {
            StopCameraAsync();

            brCameraTip.Visibility = Visibility.Collapsed;
            brLoadingOverlay.Visibility = Visibility.Collapsed;

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
                                Notification.Show(true, "Найден QR-код", $"Обнаружен '{result.Text}'");
                            }
                            else
                            {
                                Console.Beep(250, 300);
                                Notification.Show(false, "QR не распознан", $"'{result.Text}' Не существует");
                            }
                        }
                        else
                        {
                            Console.Beep(250, 300);
                            Notification.Show(false, "Ошибка", "QR-код не найден на изображении");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Notification.Show(false, "Ошибка", $"Не удалось открыть файл: {ex.Message}");
                }
            }
        }
        private void rbCamera_Checked(object sender, RoutedEventArgs e)
        {
            InitializeCameraAsync();
        }
    }
}
