using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using OpenCvSharp.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using ZXing.Windows.Compatibility;

namespace QRcodeStorage.Pages
{
    public partial class ScanQR : Page
    {
        private BarcodeReader barcodeReader;
        private VideoCapture capture;
        private DispatcherTimer timer;
        private bool isCameraReady = false;
        private bool isScanningEnabled = true;

        public ScanQR()
        {
            InitializeComponent();
            ShowLoading(true);
            InitializeCameraAsync();

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
                    MessageBox.Show($"Ошибка инициализации камеры: {ex.Message}");
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
                    MessageBox.Show($"Ошибка остановки камеры: {ex.Message}");
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
                    isScanningEnabled = false;

                    Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show($"Найден QR-код: {result.Text}");
                        isScanningEnabled = true;
                    });
                }
            }
        }


        private void ShowLoading(bool show)
        {
            LoadingOverlay.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            imgCamera.Visibility = show ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
